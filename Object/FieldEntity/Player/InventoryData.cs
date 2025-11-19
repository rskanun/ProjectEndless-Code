using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public class InventoryData : ScriptableObject
{
    // 저장 파일 위치
    private const string FILE_DIRECTORY = "Assets/Resources/Items";
    private const string FILE_PATH = "Assets/Resources/Items/Inventory.asset";

    private static InventoryData _instance;
    public static InventoryData Instance
    {
        get
        {
            if (_instance != null) return _instance;

            _instance = Resources.Load<InventoryData>("Items/Inventory");

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
                _instance = AssetDatabase.LoadAssetAtPath<InventoryData>(FILE_PATH);

                if (_instance == null)
                {
                    _instance = CreateInstance<InventoryData>();
                    AssetDatabase.CreateAsset(_instance, FILE_PATH);
                }
            }
#endif
            return _instance;
        }
    }

    // 인벤토리 데이터
    [ShowInInspector]
    private Dictionary<Item, int> inventory = new Dictionary<Item, int>();

    // 테스트 아이템
    public List<Item> testItems = new List<Item>();


    public void InitInventory()
    {
        // 임시로 아이템 채워넣기
        foreach (Item item in testItems)
        {
            if (inventory.ContainsKey(item) == false)
                inventory[item] = 1;
            else
                inventory[item]++;
        }
    }

    [ContextMenu("Inventory Clear")]
    private void Clear()
    {
        inventory = new Dictionary<Item, int>();
    }

    public void AddItem(Item item)
    {
        if (inventory.ContainsKey(item) == false)
            inventory[item] = 0;

        inventory[item]++;
    }

    public void RemoveItem(Item item)
    {
        if (HasItem(item))
        {
            inventory[item]--;
        }

        // 만약 아이템 개수가 0개로 떨어진 경우 데이터 삭제
        if (inventory[item] <= 0 && inventory.ContainsKey(item))
        {
            inventory.Remove(item);
        }
    }

    public Dictionary<Item, int> GetItems(ItemType type)
    {
        Dictionary<Item, int> result = new Dictionary<Item, int>();

        // 인벤토리 내 동일한 타입의 아이템 담기
        foreach (Item item in inventory.Keys)
        {
            if (item.Type == type)
            {
                result.Add(item, inventory[item]);
            }
        }

        return result;
    }

    public bool HasItem(Item item)
    {
        return inventory[item] > 0;
    }

    public int GetItemCount(Item item)
    {
        return inventory[item];
    }
}