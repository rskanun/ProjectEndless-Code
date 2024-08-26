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
    private string _name;
    public string Name
    {
        get { return _name; }
    }

    [SerializeField]
    private Sprite _iconSprite;
    public Sprite IconSprite
    {
        get { return _iconSprite; }
    }

    public virtual ItemType Type
    {
        get { return ItemType.Other; }
    }

    [SerializeField]
    [TextArea]
    private string _description;
    public string Description
    {
        get { return _description; }
    }
}