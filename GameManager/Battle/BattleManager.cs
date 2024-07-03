using System.Collections;
using System.Collections.Generic;
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
        battleData.SetEncounterEnemy(fieldMobData);

        // 전투 참여 엔티티 목록
        List<Entity> entityList = new List<Entity>();

        // 플레이어 진형 파티 설정
        List<Character> playerParty = GetPlayerParty();
        foreach (Character member in playerParty)
        {
            member.OnJoinBattle();
        }
        entityList.AddRange(playerParty);

        // 적 진형 파티 설정
        List<Monster> enemyParty = GetEnemyParty(fieldMobData);
        entityList.AddRange(enemyParty);

        // 시퀀스 생성
        battleSeq.SetSequence(entityList);

        // 처음 턴 진행
        StartCoroutine(RunningBattle());
    }

    private List<Character> GetPlayerParty()
    {
        PartyData partyData = PartyData.Instance;

        // 리턴될 파티 맴버 오브젝트 목록
        List<Character> partyList = new List<Character>();

        // 모든 멤버 오브젝트를 Dictionary로 변환
        Dictionary<string, Character> memberMap = new Dictionary<string, Character>();
        foreach (GameObject memberObj in allMemberObjs)
        {
            Character member = memberObj.GetComponent<Character>();
            memberMap[member.Name] = member;
        }

        // 파티 멤버 데이터를 가져와서 검색
        foreach (CharacterData memberData in partyData.GetPartyMembers())
        {
            if (memberMap.TryGetValue(memberData.Name, out Character member))
            {
                partyList.Add(member);
            }
        }

        return partyList;
    }

    private List<Monster> GetEnemyParty(FieldMobData fieldMobData)
    {
        List<Monster> enemyParty = new List<Monster>();

        foreach (GameObject prefabObj in fieldMobData.FieldMonsters)
        {
            // 적 소환
            GameObject enemyObj = Instantiate(prefabObj);

            // 소환된 적을 전투 참여 엔티티 목록에 추가
            Monster enemy = enemyObj.GetComponent<Monster>();
            enemyParty.Add(enemy);
        }

        return enemyParty;
    }

    /***************************************************************
    * [ 전투 진행 ]
    * 
    * 전투 순서에 따른 현재 턴 진행
    ***************************************************************/

    private IEnumerator RunningBattle()
    {
        battleData.IsInBattle = battleData.EnemyCount > 0;

        // 전투가 진행되는 동안 각자의 턴 진행
        while (battleData.IsInBattle)
        {
            timeline.Print();

            TakeTurn();
            yield return new WaitUntil(() => isTurnEnded);
        }

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
        if (battleData.EnemyCount <= 0 || battleData.IsInBattle == false)
        {
            // 모든 적을 해치웠거나, 전투가 끝난 경우 전투 종료
            battleData.IsInBattle = false;
        }
        else
        {
            // 계속 전투 중일 경우 다음 턴 진행
            battleSeq.NextTurn();
        }

        // 턴 끝내기
        isTurnEnded = true;
    }

    private void EndBattle()
    {
        if (battleData.EnemyCount <= 0)
        {
            // 모든 적을 해치운 경우 보상 지급
        }

        // 전투 종료
    }
}