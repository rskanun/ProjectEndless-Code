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
    private List<Monster> _enemyList = new List<Monster>();
    public List<Monster> EnemyList
    {
        get { return _enemyList; }
        private set { _enemyList = value; }
    }

    [ReadOnly]
    [SerializeField]
    private List<Monster> _enemyFrontList = new List<Monster>();
    public List<Monster> EnemyFrontList
    {
        get { return _enemyFrontList; }
        private set { _enemyFrontList = value; }
    }

    [Header("아군 정보")]
    [ReadOnly]
    [SerializeField]
    private List<Character> _characterList = new List<Character>();
    public List<Character> CharacterList
    {
        get { return _characterList; }
        private set { _characterList = value; }
    }

    [ReadOnly]
    [SerializeField]
    private List<Character> _characterFrontList = new List<Character>();
    public List<Character> CharacterFrontList
    {
        get { return _characterFrontList; }
        private set { _characterFrontList = value; }
    }

    [Header("전투 정보")]
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

    public bool IsInBattle
    {
        get
        {
            // 적이나 주인공 파티 맴버가 남아있다면 전투를 지속하는 것으로 판단
            return IsLivingEnemy && IsLivingCharacter;
        }
    }
    public bool IsLivingEnemy
    {
        get
        {
            List<Entity> list = new List<Entity>(EnemyList);

            return IsLivingEntity(list);
        }
    }
    public bool IsLivingEnemyFront
    {
        get
        {
            List<Entity> list = new List<Entity>(EnemyFrontList);

            return IsLivingEntity(list);
        }
    }
    public bool IsLivingCharacter
    {
        get
        {
            List<Entity> list = new List<Entity>(CharacterList);

            return IsLivingEntity(list);
        }
    }
    public bool IsLivingCharacterFront
    {
        get
        {
            List<Entity> list = new List<Entity>(CharacterFrontList);

            return IsLivingEntity(list);
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
    private Dictionary<Item, int> _dropItems = new Dictionary<Item, int>();
    public Dictionary<Item, int> DropItems
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
        CharacterList.Clear();
        CharacterFrontList.Clear();

        // 보상 정보 초기화
        ClearReward();
    }

    public void ClearReward()
    {
        TotalAmount = 0;
        DropItems.Clear();
    }

    public void SetEnemyList(List<Monster> encountEnemys)
    {
        // 새로운 적에 대한 데이터 삽입
        EnemyList = encountEnemys;

        // 전위에 대한 데이터 삽입
        foreach (Monster enemy in encountEnemys)
        {
            if (enemy.Position == BattlePosition.Front)
            {
                EnemyFrontList.Add(enemy);
            }
        }
    }

    public void SetPartyList(List<Character> party)
    {
        // 파티에 대한 데이터 삽입
        CharacterList = party;

        // 전위에 대한 데이터 삽입
        foreach (Character character in party)
        {
            if (character.Position == BattlePosition.Front)
            {
                CharacterFrontList.Add(character);
            }
        }
    }

    public void AddKillReward(Monster enemy)
    {
        Dictionary<Item, int> items = enemy.GetDropItems();

        // 해당 몬스터의 처지 보상 저장
        TotalAmount += enemy.GetDropGold();
        foreach (Item item in items.Keys)
        {
            if (DropItems.ContainsKey(item))
            {
                DropItems[item] += items[item];
            }
            else
            {
                DropItems[item] = items[item];
            }
        }
    }

    public void RemoveEntity(Entity entity)
    {
        if (entity is Monster) RemoveEnemyData((Monster) entity);
        else RemoveCharacterData((Character) entity);
    }

    private void RemoveEnemyData(Monster enemy)
    {
        // 전위에 포함되어 있을 경우 삭제
        if (EnemyFrontList.Contains(enemy))
        {
            EnemyFrontList.Remove(enemy);
        }

        // 몬스터 목록에서 삭제
        if (EnemyList.Contains(enemy))
        {
            EnemyList.Remove(enemy);
        }
    }

    private void RemoveCharacterData(Character character)
    {
        // 전위에 포함되어 있을 경우 삭제
        if (CharacterFrontList.Contains(character))
        {
            CharacterFrontList.Remove(character);
        }

        // 캐릭터 목록에서 삭제
        if (CharacterList.Contains(character))
        {
            CharacterList.Remove(character);
        }
    }

    private bool IsLivingEntity(List<Entity> entityList)
    {
        foreach (Entity entity in entityList)
        {
            // 한 명이라도 살아있을 경우 살아있음 리턴
            if (entity.IsDead == false) return true;
        }

        return false;
    }
}