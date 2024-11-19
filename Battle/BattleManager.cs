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
    [SerializeField] private BattleResultUI resultUI;
    [SerializeField] private TargetSelection selectionManager;

    [Header("엔티티 배치")]
    [SerializeField]
    private PositionData battlePos;
    private Dictionary<(BattlePosition, int), Vector2> position;

    [Header("캐릭터 오브젝트")]
    [SerializeField] private List<GameObject> allMemberObjs;

    [Header("테스트 필드 몬스터")]
    [SerializeField] private BattleFieldData fieldData;

    // 참조 데이터
    private CurrentBattleData battleData;
    private BattleSequence battleSeq;

    // 전투 진행 상태
    private bool isTurnEnded = false;
    private List<Entity> entityList = new List<Entity>();

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
        battleData = CurrentBattleData.Instance;
        battleSeq = battleData.Sequence;

        // 임시 몹 데이터 집어넣기
        OnEncounter(fieldData);
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
        // 전투 참여 엔티티 목록 초기화
        entityList.Clear();

        // 플레이어 진형 파티 설정
        List<Character> playerParty = GetPlayerParty();
        ReadyToJoinBattle(playerParty);
        entityList.AddRange(playerParty);

        // 적 진형 파티 설정
        List<Monster> enemyParty = GetEnemyParty(fieldData.FieldMonsters);
        entityList.AddRange(enemyParty);

        // 전투 데이터 초기화
        battleData.Clear();

        // 전투에 참여하는 엔티티 목록 설정
        battleData.SetEnemyList(enemyParty);
        battleData.SetPartyList(playerParty);

        // 시퀀스 생성
        battleSeq.SetSequence(entityList);

        // 타임라인 생성
        timeline.SetupTimeline(battleSeq);

        // 선택 버튼 등록
        selectionManager.InitSelectableEntities();

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
            .Select(memberData => memberMap[memberData.Name])
            .ToList();
    }

    private void ReadyToJoinBattle(List<Character> playerParty)
    {
        foreach (Character member in playerParty)
        {
            member.OnJoinBattle();
        }
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

    /***************************************************************
    * [ 전투 진행 ]
    * 
    * 전투 순서에 따른 현재 턴 진행
    ***************************************************************/

    private IEnumerator RunningBattle()
    {
        // 전투가 진행되는 동안 각자의 턴 진행
        while (battleData.IsInBattle)
        {
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
        isTurnEnded = false;

        // 턴 시작 알림
        GameEventResource.Instance.StartTurnEvent.NotifyUpdate();

        // 이전에 입력한 행동 실행
        BattleAction curAction = battleSeq.GetTurnAction(0);
        curAction.OnAction();

        // 이전 행동 모션이 끝날 때까지 대기
        yield return new WaitUntil(() => !curAction.actor.IsActionable);

        // 다음 턴에 진행할 행동 선택
        curAction.actor.TakeTurn();
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
        if (battleData.IsLivingEnemy)
        {
            // 몬스터가 살아있을 경우 = 플레이어가 도망쳤을 경우
            // 처치 보상 X
            battleData.ClearReward();
        }

        // 전투가 끝났다면 결과창 출력
        resultUI.OpenResult();
    }

    private void UpdateEffectTimers(float turn)
    {
        foreach (Entity entity in entityList)
        {
            entity.UpdateEffectTimer(turn);
        }
    }
}