using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [System.Serializable]
    private class PositionData
    {
        public List<Vector2> front;
        public List<Vector2> back;
    }

    [Header("참조 스크립트")]
    [SerializeField] private Timeline timeline;
    [SerializeField] private ResultWindow resultUI;
    [SerializeField] private TargetSelection selectionManager;

    [Header("엔티티 배치")]
    [SerializeField]
    private PositionData battlePos;
    private Dictionary<(BattlePosition, int), Vector2> position;

    [Header("캐릭터 오브젝트")]
    [SerializeField] private List<GameObject> allMemberObjs;

    [Header("전투 데이터")]
    public BattleData battleData;
    private BattleSequence battleSeq;

    private BattleCameraDirector director;

    // 전투 진행 상태
    private bool isTurnEnded = false;

    private void InitPosition()
    {
        position = new Dictionary<(BattlePosition, int), Vector2>();

        // 전열, 후열 위치 설정
        SetPositions(BattlePosition.Front, battlePos.front);
        SetPositions(BattlePosition.Back, battlePos.back);
    }

    private void SetPositions(BattlePosition posType, List<Vector2> posList)
    {
        for (int i = 0; i < posList.Count; i++)
        {
            position[(posType, i)] = posList[i];
        }
    }

    private void Start()
    {
        director = BattleCameraDirector.Instance;
        battleData = BattleData.Instance;
        battleSeq = battleData.Sequence;

        // 임시로 일반 전투 실행
        OnEncounter(BattleCache.Current.FieldData);
    }

    /***************************************************************
    * [ 전투 순서 ]
    * 
    * 현재 상황에 따른 전투 진행 순서 처리
    ***************************************************************/

    public void OnEncounter(BattleFieldData fieldData)
    {
        // 일반 전투 시작
        StartBattle(fieldData);
    }

    public void OnAmbushEnemy(BattleFieldData fieldData)
    {
        // 적을 기습했을 때의 전투 시작
    }

    public void OnAmushPlayer(BattleFieldData fieldData)
    {
        // 적에게 기습당했을 때의 전투 시작
    }

    private void StartBattle(BattleFieldData fieldData)
    {
        // 플레이어 진형 파티 설정
        List<Character> playerParty = GetPlayerParty();
        InitPlayerParty(playerParty);

        // 적 진형 파티 설정
        List<Monster> enemyParty = GetEnemyParty(fieldData.EncountMonsters);
        InitEnemyParty(enemyParty);

        // 전투에 참여하는 엔티티 목록 설정
        battleData.SetEnemyList(enemyParty);

        // 시퀀스 생성
        List<Entity> entityList = playerParty.Concat<Entity>(enemyParty).ToList();
        battleSeq.SetSequence(entityList);

        // 타임라인 생성
        timeline.SetupTimeline(battleSeq);

        // 개전 시작 알림
        GameEventManager.Instance.NotifyBattleStarted();

        // 처음 턴 진행
        StartCoroutine(RunningBattle());
    }

    private List<Character> GetPlayerParty()
    {
        PartyData partyData = PartyData.Instance;

        // 모든 멤버 오브젝트를 Dictionary로 변환
        Dictionary<string, Character> memberMap = allMemberObjs
            .Select(memberObj => memberObj.GetComponent<Character>())
            .ToDictionary(member => member.Name);

        // 파티 멤버 데이터를 가져와서 검색
        return partyData.GetPartyMembers()
            .Where(memberData => memberMap.TryGetValue(memberData.Name, out _))
            .Select(memberData =>
            {
                Character chr = memberMap[memberData.Name];

                // 전투 돌입 시의 설정
                chr.OnJoinBattle();

                return chr;
            }).ToList();
    }

    private List<Monster> GetEnemyParty(List<GameObject> mobList)
    {
        return mobList.Select(prefabObj =>
            {
                // 적 소환
                GameObject enemyObj = Instantiate(prefabObj);

                // 해당 적의 몬스터 객체 리턴
                return enemyObj.GetComponent<Monster>();
            }).ToList();
    }

    private void InitPlayerParty(List<Character> party)
    {
        // 카메라 셋팅
        foreach (Entity chr in party)
        {
            int instanceID = chr.gameObject.GetInstanceID();
            Transform bodyPivot = chr.cameraOption.BodyPivot;

            director.RegisterPlayerChrPivot(instanceID, bodyPivot);
        }

        // 전투에 참여하는 엔티티 목록 설정
        battleData.SetPartyList(party);
    }

    private void InitEnemyParty(List<Monster> party)
    {
        // 카메라 셋팅
        foreach (Entity chr in party)
        {
            int instanceID = chr.gameObject.GetInstanceID();
            Transform bodyPivot = chr.cameraOption.BodyPivot;

            director.RegisterEnemyChrPivot(instanceID, bodyPivot);
        }

        // 전투에 참여하는 엔티티 목록 설정
        battleData.SetEnemyList(party);
    }

    /***************************************************************
    * [ 전투 진행 ]
    * 
    * 전투 순서에 따른 현재 턴 진행
    ***************************************************************/

    private IEnumerator RunningBattle()
    {
        yield return director.DirectBattleStart();

        // 전투가 진행되는 동안 각자의 턴 진행
        while (battleData.IsInBattle)
        {
            isTurnEnded = false;

            // 턴 진행
            StartCoroutine(TakeTurn());

            // 턴이 끝날 때까지 대기
            yield return new WaitUntil(() => isTurnEnded);
        }

        // 전투 끝내기
        EndBattle();
    }

    private IEnumerator TakeTurn()
    {
        BattleAction curAction = battleSeq.GetTurnAction(0);
        Entity actor = curAction.actor;

        // 턴 시작 알림
        GameEventManager.Instance.NotifyTurnStarted();

        // 이전에 입력한 행동 실행
        curAction.OnAction();

        // 이전 행동 모션이 끝날 때까지 대기
        yield return new WaitUntil(() => actor.IsIdle);

        // 해당 행동으로 전투가 끝났거나, 해당 엔티티가 사망한 경우 턴 끝내기
        if (battleData.IsInBattle == false || actor.IsDead)
        {
            // 만약 사망으로 턴을 끝내는 경우
            if (actor.IsDead)
            {
                // 다음 턴(=이번 턴) 턴수만큼 상태이상 지속 시간 돌리기
                BattleAction nextAction = battleSeq.GetTurnAction(0);
                UpdateEffectTimers(nextAction.remainTurn);
            }

            isTurnEnded = true;
            yield break;
        }

        // 다음 턴에 진행할 행동 선택
        actor.TakeTurn();
    }

    public void EndTurn()
    {
        // 적이 남아있다면 다음 턴 진행
        if (battleData.IsInBattle)
        {
            // 다음 턴 턴수만큼 상태이상 지속 시간 돌리기
            BattleAction nextAction = battleSeq.GetTurnAction(1);
            UpdateEffectTimers(nextAction.remainTurn);

            // 다음 턴 진행
            battleSeq.NextTurn();
        }

        // 턴 끝내기
        isTurnEnded = true;
    }

    private void EndBattle()
    {
        if (!battleData.IsLivingCharacter)
        {
            // 플레이어 파티가 죽거나 도망간 경우
            // 처치 보상 X
            battleData.ClearReward();
        }

        // 전투 종료 알림
        GameEventManager.Instance.NotifyBattleEnded();

        // 전체 화면으로 카메라 포커싱
        BattleCameraDirector.Instance.FocusFullScreen();

        // 전투 결과 캐시 저장
        BattleCache.Current.Result = GetBattleResult();

        // 전투가 끝났다면 결과창 출력
        resultUI.OpenResult();
    }

    private BattleResult GetBattleResult()
    {
        if (battleData.IsLivingCharacter) // 플레이어 파티가 살아있다면 승리
            return BattleResult.Victory;
        else if (battleData.CharacterList.Count > 0) // 전부 사망 판정에 필드에 남아있다면 패배
            return BattleResult.Defeat;
        else // 필드에 한 명도 남아있지 않다면 도망
            return BattleResult.Escape;
    }

    private void UpdateEffectTimers(float turn)
    {
        // 캐릭터 버프 시간 돌리기
        foreach (Entity entity in battleData.CharacterList)
        {
            entity.UpdateEffectTimer(turn);
        }

        // 적 버프 시간 돌리기
        foreach (Entity entity in battleData.EnemyList)
        {
            entity.UpdateEffectTimer(turn);
        }
    }
}