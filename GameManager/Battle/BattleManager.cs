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

    [Header("캐릭터 오브젝트")]
    [SerializeField] private List<GameObject> allMemberObjs;

    [Header("엔티티 배치")]
    [SerializeField]
    private PositionData battlePos;
    private Dictionary<(BattlePosition, int), Vector2> position;

    [Header("참조 스크립트")]
    [SerializeField] private Timeline timeline;
    [SerializeField] private BattleResultUI resultUI;
    [SerializeField] private SelectionManager selectionManager;

    [Header("테스트 필드 몬스터")]
    [SerializeField] private FieldMobData mobData;

    // 참조 데이터
    private BattleData battleData;
    private BattleSequence battleSeq;

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
        battleData = BattleData.Instance;
        battleSeq = battleData.Sequence;

        // 임시 몹 데이터 집어넣기
        OnEncounter(mobData);
    }

    /***************************************************************
    * [ 전투 순서 ]
    * 
    * 현재 상황에 따른 전투 진행 순서 처리
    ***************************************************************/

    public void OnEncounter(FieldMobData fieldMobData)
    {
        // 일반 전투 시작
        StartBattle(fieldMobData);
    }

    public void OnAmbushEnemy(FieldMobData fieldMobData)
    {
        // 적을 기습했을 때의 전투 시작
    }

    public void OnAmushPlayer(FieldMobData fieldMobData)
    {
        // 적에게 기습당했을 때의 전투 시작
    }

    private void StartBattle(FieldMobData fieldMobData)
    {
        // 전투 참여 엔티티 목록
        List<Entity> entityList = new List<Entity>();

        // 플레이어 진형 파티 설정
        List<Character> playerParty = GetPlayerParty();
        ReadyToJoinBattle(playerParty);
        entityList.AddRange(playerParty);

        // 적 진형 파티 설정
        List<Monster> enemyParty = GetEnemyParty(fieldMobData);
        entityList.AddRange(enemyParty);

        // 전투 데이터 초기화
        battleData.Clear();

        // 전투에 참여하는 엔티티 목록 설정
        battleData.SetEncounterEnemy(GetPartyObjs(enemyParty));
        battleData.SetPartyList(GetPartyObjs(playerParty));

        // 시퀀스 생성
        battleSeq.SetSequence(entityList);

        // 타임라인 생성
        timeline.InitTimeline(battleSeq);

        // 선택 버튼 생성
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

    private List<Monster> GetEnemyParty(FieldMobData fieldMobData)
    {
        return fieldMobData.FieldMonsters
            .Select(prefabObj =>
            {
                // 적 소환
                GameObject enemyObj = Instantiate(prefabObj);

                // 해당 적의 몬스터 객체 리턴
                return enemyObj.GetComponent<Monster>();
            }).ToList();
    }

    private List<GameObject> GetPartyObjs<T>(List<T> partyList) where T : Entity
    {
        return partyList.Select(entity => entity.gameObject).ToList();
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
            // 맨 앞의 타임라인 표식 갱신
            timeline.MarkCurIcon();

            // 턴 진행
            TakeTurn();

            // 턴이 끝날 때까지 대기
            yield return new WaitUntil(() => isTurnEnded);

            // 타임라인 업데이트
            timeline.UpdateTimeline();
        }

        // 전투 끝내기
        EndBattle();
    }

    private void TakeTurn()
    {
        // 턴 진행
        isTurnEnded = false;

        // 이전에 입력한 행동 실행
        BattleAction curAction = battleSeq.GetCurrentTurn();
        curAction.OnAction();

        // 다음 턴에 진행할 행동 선택
        curAction.actor.TakeTurn();
    }

    public void EndTurn()
    {
        if (battleData.IsInBattle)
        {
            // 적이 남아있다면 다음 턴 진행
            battleSeq.NextTurn();
        }

        // 턴 끝내기
        isTurnEnded = true;
    }

    private void EndBattle()
    {
        if (battleData.PartyMemberCount > 0)
        {
            // 파티가 살아남았다면 결과창 출력
            resultUI.OpenResult();
        }
    }
}