using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
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
    [SerializeField]
    private List<GameObject> allMemberObjs;
    private Dictionary<string, Character> memberLookup;

    // 전투 진행 상태
    private bool isTurnEnded = false;
    public BattleData battleData;

    private void OnValidate()
    {
        memberLookup = allMemberObjs
            .Select(memberObj => memberObj?.GetComponent<Character>())
            .Where(member => member != null)
            .ToDictionary(member => member.Name);
    }

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
        battleData = BattleData.Instance;

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
        var battleSeq = BattleData.Instance.Sequence;

        // 플레이어 진형 파티 설정
        List<Character> playerParty = GetPlayerParty();
        InitPlayerParty(playerParty);

        // 적 진형 파티 설정
        List<Monster> enemyParty = SummonEnemyParty(fieldData.EncountMonsters);
        InitEnemyParty(enemyParty);

        // 전투에 참여하는 엔티티 목록 설정
        BattleData.Instance.SetEnemyList(enemyParty);

        // 시퀀스 생성
        List<Entity> entityList = playerParty.Concat<Entity>(enemyParty).ToList();
        battleSeq.SetSequence(entityList);

        // 타임라인 생성
        timeline.SetupTimeline(battleSeq);

        // 개전 시작 알림
        GameEventManager.Instance.NotifyBattleStarted();

        // 처음 턴 진행
        RunningBattle().Forget();
    }

    private List<Character> GetPlayerParty()
    {
        // 파티 멤버 데이터를 가져와서 검색
        return PartyData.Instance.GetPartyMembers()
            .Where(memberData => memberLookup.TryGetValue(memberData.Name, out _))
            .Select(memberData => memberLookup[memberData.Name])
            .ToList();
    }

    private List<Monster> SummonEnemyParty(List<GameObject> mobList)
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
        foreach (Character chr in party)
        {
            // 전투 돌입 셋팅
            chr.OnJoinBattle();

            // 카메라 셋팅
            int instanceID = chr.gameObject.GetInstanceID();
            Transform bodyPivot = chr.cameraOption.BodyPivot;

            BattleCameraDirector.Instance.RegisterPlayerChrPivot(instanceID, bodyPivot);
        }

        // 전투에 참여하는 엔티티 목록 설정
        BattleData.Instance.SetPartyList(party);
    }

    private void InitEnemyParty(List<Monster> party)
    {
        // 카메라 셋팅
        foreach (Entity chr in party)
        {
            int instanceID = chr.gameObject.GetInstanceID();
            Transform bodyPivot = chr.cameraOption.BodyPivot;

            BattleCameraDirector.Instance.RegisterEnemyChrPivot(instanceID, bodyPivot);
        }

        // 전투에 참여하는 엔티티 목록 설정
        BattleData.Instance.SetEnemyList(party);
    }

    /***************************************************************
    * [ 전투 진행 ]
    * 
    * 전투 순서에 따른 현재 턴 진행
    ***************************************************************/

    private async UniTask RunningBattle()
    {
        await BattleCameraDirector.Instance.DirectBattleStart();

        // 전투가 진행되는 동안 각자의 턴 진행
        while (BattleData.Instance.IsInBattle)
        {
            isTurnEnded = false;

            // 턴 진행
            await TakeTurn();

            // 턴이 끝날 때까지 대기
            await UniTask.WaitUntil(() => isTurnEnded);
        }

        // 전투 끝내기
        EndBattle();
    }

    private async UniTask TakeTurn()
    {
        var battleSeq = BattleData.Instance.Sequence;

        BattleAction curAction = battleSeq.GetTurnAction(0);
        Entity actor = curAction.actor;

        // 턴 시작 알림
        GameEventManager.Instance.NotifyTurnStarted();

        // 이전에 입력한 행동 실행
        curAction.OnAction();

        // 이전 행동 모션이 끝날 때까지 대기
        await UniTask.WaitUntil(() => actor.IsIdle);

        // 해당 행동으로 전투가 끝났거나, 해당 엔티티가 사망한 경우 턴 끝내기
        if (BattleData.Instance.IsInBattle == false || actor.IsDead)
        {
            // 만약 사망으로 턴을 끝내는 경우
            if (actor.IsDead)
            {
                // 다음 턴(=이번 턴) 턴수만큼 상태이상 지속 시간 돌리기
                BattleAction nextAction = battleSeq.GetTurnAction(0);
                UpdateEffectTimers(nextAction.remainTurn);
            }

            isTurnEnded = true;
            return;
        }

        // 다음 턴에 진행할 행동 선택
        actor.TakeTurn();
    }

    public void EndTurn()
    {
        // 적이 남아있다면 다음 턴 진행
        if (BattleData.Instance.IsInBattle)
        {
            var battleSeq = BattleData.Instance.Sequence;

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
        if (!BattleData.Instance.IsLivingCharacter)
        {
            // 플레이어 파티가 죽거나 도망간 경우
            // 처치 보상 X
            BattleData.Instance.ClearReward();
        }

        // 전투 종료 시 상태를 현재 상태로 갱신
        UpdateCharacterStats();

        // 전투 종료 알림
        GameEventManager.Instance.NotifyBattleEnded();

        // 전체 화면으로 카메라 포커싱
        BattleCameraDirector.Instance.FocusFullScreen();

        // 전투 결과 캐시 저장
        BattleCache.Current.Result = GetBattleResult();

        // 전투가 끝났다면 결과창 출력
        resultUI.OpenResult();
    }

    private void UpdateCharacterStats()
    {
        var partyData = PartyData.Instance;

        // 전투에 참여한 캐릭터만 스탯 업데이트
        foreach (var character in BattleData.Instance.CharacterList)
        {
            var originData = partyData.GetCharacter(character.Name);

            // HP, SP 적용
            originData.Stats.HP = character.FinalStats.HP;
            originData.Stats.SP = character.FinalStats.SP;
        }
    }

    private BattleResult GetBattleResult()
    {
        if (BattleData.Instance.IsLivingCharacter) // 플레이어 파티가 살아있다면 승리
            return BattleResult.Victory;
        else if (BattleData.Instance.CharacterList.Count > 0) // 전부 사망 판정에 필드에 남아있다면 패배
            return BattleResult.Defeat;
        else // 필드에 한 명도 남아있지 않다면 도망
            return BattleResult.Escape;
    }

    private void UpdateEffectTimers(float turn)
    {
        // 캐릭터 버프 시간 돌리기
        foreach (Entity entity in BattleData.Instance.CharacterList)
        {
            entity.UpdateEffectTimer(turn);
        }

        // 적 버프 시간 돌리기
        foreach (Entity entity in BattleData.Instance.EnemyList)
        {
            entity.UpdateEffectTimer(turn);
        }
    }
}