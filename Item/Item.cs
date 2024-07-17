using UnityEngine;

public enum ItemType
{
    Consumable, // 소모성 아이템
    Weapon,     // 무기
    Armor,      // 방어구
    Quest,      // 퀘스트 아이템
    Other       // 기타
}

[CreateAssetMenu(menuName = "Item/Other", fileName = "Other Item")]
public class Item : ScriptableObject
{
    [SerializeField]
    private string _itemName;
    public string Name
    {
        get { return _itemName; }
    }

    [SerializeField]
    private Sprite _icon;
    public Sprite Icon
    {
        get { return _icon; }
    }

    public virtual ItemType Type
    {
        get { return ItemType.Other; }
    }

    [SerializeField]
    [TextArea]
    private string _lores;
    public string Lores
    {
        get { return _lores; }
    }
}