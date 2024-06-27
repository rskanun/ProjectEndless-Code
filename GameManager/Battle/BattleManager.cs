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

    [Header("테스트 필드 몬스터")]
    [SerializeField] private FieldMobData mobData;

    // 현재 상황
    private BattleSeq sequence;

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
        // 임시 몹 데이터 집어넣기
        OnBattle(mobData);
    }

    /***************************************************************
    * [ 전투 순서 ]
    * 
    * 현재 상황에 따른 전투 진행 순서 처리
    ***************************************************************/

    public void OnBattle(FieldMobData fieldMobData)
    {
        BattleData.Instance.EnemyData = fieldMobData;

        // 전투 참여 엔티티 목록
        List<Entity> entityList = new List<Entity>();

        // 플레이어 진형 파티 설정
        List<Character> playerParty = GetPartyMember();
        foreach (Character member in playerParty)
        {
            member.OnJoinBattle();
        }

        entityList.AddRange(playerParty);

        // 적 진형 파티 설정
        List<Monster> enemyParty = new List<Monster>();
        foreach (GameObject enemyObj in fieldMobData.FieldMonsters)
        {
            // 적 소환
            Instantiate(enemyObj);

            // 소환된 적을 전투 참여 엔티티 목록에 추가
            Monster enemy = enemyObj.GetComponent<Monster>();
            enemyParty.Add(enemy);
        }

        entityList.AddRange(enemyParty);

        // 시퀀스 생성
        sequence = new BattleSeq(entityList);

        // 처음 턴 진행
        TakeTurn();
    }

    public void OnAmbushEnemy(FieldMobData fieldMobData)
    {
        // 적을 기습했을 때의 전투 시작
    }

    public void OnAmushPlayer(FieldMobData fieldMobData)
    {
        // 적에게 기습당했을 때의 전투 시작
    }

    private List<Character> GetPartyMember()
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

    /***************************************************************
    * [ 전투 진행 ]
    * 
    * 전투 순서에 따른 현재 턴 진행
    ***************************************************************/

    private void TakeTurn()
    {
        // 턴 진행
        BattleAction curAction = sequence.GetCurrentTurn();
        curAction.OnAction();
    }

    public void SetTurn(BattleAction action)
    {
        // 다음 자신의 턴에 진행할 행동 설정
        sequence.SetTurn(action);

        // 다음 턴 넘어가기
        NextTurn();
    }

    private void NextTurn()
    {
        sequence.NextTurn();

        // 다음 턴 진행
        TakeTurn();
    }
}