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

    // 현재 상황
    private BattleSeq sequence;
    private int mobCount;
    private bool isAmbushPlayer;    // 플레이어가 기습을 당했는지 여부
    private bool isAmbushEnemy;   // 적이 기습을 당했는지 여부

    private void Awake()
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

    /***************************************************************
    * [ 전투 순서 ]
    * 
    * 현재 상황에 따른 전투 진행 순서 처리
    ***************************************************************/

    public void OnBattle(FieldMobData fieldMobData)
    {
        // 처리해야 할 적 숫자 설정
        mobCount = fieldMobData.FieldMonsterObjs.Count;

        // 전투 참여 엔티티 목록
        List<Entity> entityList = GetPartyObjs();
        entityList.AddRange(fieldMobData.GetFieldMobs());

        // 시퀀스 생성
        sequence = new BattleSeq(entityList);

        // 처음 턴 진행
        BattleAction curAction = sequence.GetCurrentTurn();
        TakeAction(curAction);
    }

    public void OnAmbushEnemy(FieldMobData fieldMobData)
    {
        // 적을 기습했을 때의 전투 시작
    }

    public void OnAmushPlayer(FieldMobData fieldMobData)
    {
        // 적에게 기습당했을 때의 전투 시작
    }

    private List<Entity> GetPartyObjs()
    {
        PartyData partyData = PartyData.Instance;

        // 리턴될 파티 맴버 오브젝트 목록
        List<Entity> partyList = new List<Entity>();

        // 모든 멤버 오브젝트를 Dictionary로 변환
        Dictionary<string, Entity> memberMap = new Dictionary<string, Entity>();
        foreach (GameObject memberObj in allMemberObjs)
        {
            Entity member = memberObj.GetComponent<Character>();
            memberMap[member.Name] = member;
        }

        // 파티 멤버 데이터를 가져와서 검색
        foreach (CharacterData memberData in partyData.GetPartyMembers())
        {
            if (memberMap.TryGetValue(memberData.Name, out Entity member))
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

    public void TakeAction(BattleAction action)
    {
        // 예약해둔 턴의 행동 실행
        action.OnAction();
    }

    public void SetTurn(BattleAction action)
    {
        // 다음 자신의 턴에 진행할 행동 설정
        sequence.SetTurn(action);

        // 다음 턴 넘어가기
    }

    private void NextTurn()
    {
        sequence.NextTurn();

        // 다음 턴 진행
        BattleAction curAction = sequence.GetCurrentTurn();
        TakeAction(curAction);
    }
}