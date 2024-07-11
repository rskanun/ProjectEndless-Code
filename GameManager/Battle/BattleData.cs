using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BattleData : ScriptableObject
{
    // 저장 파일 위치
    private const string FILE_DIRECTORY = "Assets/Resources/InGameData/Battle";
    private const string FILE_PATH = "Assets/Resources/InGameData/Battle/BattleData.asset";

    private static BattleData _instance;
    public static BattleData Instance
    {
        get
        {
            if (_instance != null) return _instance;

            _instance = Resources.Load<BattleData>("InGameData/Battle/BattleData");

#if UNITY_EDITOR
            if (_instance == null)
            {
                // 파일 경로가 없을 경우 폴더 생성
                if (!AssetDatabase.IsValidFolder(FILE_DIRECTORY))
                {
                    string[] folders = FILE_DIRECTORY.Split('/');
                    string currentPath = folders[0];

                    for (int i = 1; i < folders.Length; i++)
                    {
                        if (!AssetDatabase.IsValidFolder(currentPath + "/" + folders[i]))
                        {
                            AssetDatabase.CreateFolder(currentPath, folders[i]);
                        }

                        currentPath += "/" + folders[i];
                    }
                }

                // Resource.Load가 실패했을 경우
                _instance = AssetDatabase.LoadAssetAtPath<BattleData>(FILE_PATH);

                if (_instance == null)
                {
                    _instance = CreateInstance<BattleData>();
                    AssetDatabase.CreateAsset(_instance, FILE_PATH);
                }
            }
#endif
            return _instance;
        }
    }

    [Header("적 정보")]
    [ReadOnly]
    [SerializeField]
    private List<GameObject> _enemyList = new List<GameObject>();
    public List<GameObject> EnemyList
    {
        private set { _enemyList = value; }
        get { return _enemyList; }
    }
    public int EnemyCount
    {
        get { return EnemyList.Count; }
    }

    [ReadOnly]
    [SerializeField]
    private List<GameObject> _enemyFrontList = new List<GameObject>();
    public List<GameObject> EnemyFrontList
    {
        private set { _enemyFrontList = value; }
        get { return _enemyFrontList; }
    }
    public int EnemyFrontCount
    {
        get { return EnemyFrontList.Count; }
    }

    [Header("아군 정보")]
    [ReadOnly]
    [SerializeField]
    private List<GameObject> _partyList = new List<GameObject>();
    public List<GameObject> PartyList
    {
        private set { _partyList = value; }
        get { return _partyList; }
    }
    public int MemberCount
    {
        get { return PartyList.Count; }
    }

    [ReadOnly]
    [SerializeField]
    private List<GameObject> _partyFrontList = new List<GameObject>();
    public List<GameObject> PartyFrontList
    {
        private set { _partyFrontList = value; }
        get { return _partyFrontList; }
    }
    public int PartyFrontCount
    {
        get { return PartyFrontList.Count; }
    }

    [Header("전투 정보")]
    [ReadOnly]
    [SerializeField]
    private bool _isInBattle = false;
    public bool IsInBattle
    {
        set { _isInBattle = value; }
        get { return _isInBattle; }
    }

    [SerializeField]
    private BattleSequence _sequence;
    public BattleSequence Sequence
    {
        get
        {
            if (_sequence == null)
                _sequence = new BattleSequence();

            return _sequence;
        }
    }

    [ReadOnly]
    [SerializeField]
    private int _totalAmount;
    public int TotalAmount
    {
        private set { _totalAmount = value; }
        get { return _totalAmount; }
    }
    [ReadOnly]
    [SerializeField]
    private List<Item> _dropItems = new List<Item>();
    public List<Item> DropItems
    {
        private set { _dropItems = value; }
        get { return _dropItems; }
    }

    public void Clear()
    {
        // 적 정보 초기화
        EnemyList.Clear();
        EnemyFrontList.Clear();

        // 아군 정보 초기화
        PartyList.Clear();
        PartyFrontList.Clear();

        // 전투 정보 초기화
        IsInBattle = false;

        // 보상 정보 초기화
        TotalAmount = 0;
        DropItems.Clear();
    }

    public void SetEncounterEnemy(List<GameObject> encountEnemys)
    {
        // 새로운 적에 대한 데이터 삽입
        EnemyList = encountEnemys;
        foreach (GameObject enemyObj in encountEnemys)
        {
            BattlePosition position = enemyObj.GetComponent<Monster>().Position;

            if (position.Equals(BattlePosition.Front))
            {
                // 전위의 경우 전위 목록에도 추가
                EnemyFrontList.Add(enemyObj);
            }
        }
    }

    public void SetPartyList(List<GameObject> party)
    {
        foreach (GameObject partyMemeber in party)
        {
            PartyList.Add(partyMemeber);

            BattlePosition position = partyMemeber.GetComponent<Character>().Position;

            if (position.Equals(BattlePosition.Front))
            {
                // 전위의 경우 전위 목록에도 추가
                PartyFrontList.Add(partyMemeber);
            }
        }
    }

    public void KilledEnemy(Monster enemy)
    {
        // 필드 몬스터일 경우에만 삭제
        if (EnemyList.Contains(enemy.gameObject))
        {
            // 필드 몬스터 목록에서 삭제
            RemoveEnemyData(enemy);

            // 해당 몬스터의 처지 보상 저장
            TotalAmount += enemy.GetDropGold();
            DropItems.AddRange(enemy.GetDropItems());
        }
    }

    private void RemoveEnemyData(Monster enemy)
    {
        // 필드 몬스터 목록에서 삭제
        EnemyList.Remove(enemy.gameObject);

        // 전투 시퀀스 내에서 예약해둔 행동 삭제
        Debug.Log(enemy.Name);
        Sequence.RemoveTurns(enemy);

        // 전위의 경우 전위 목록에서도 삭제
        BattlePosition position = enemy.Position;
        if (position.Equals(BattlePosition.Front))
        {
            EnemyFrontList.Remove(enemy.gameObject);
        }
    }
}