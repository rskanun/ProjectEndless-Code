using UnityEngine;

public enum Type
{
    Consumable, // 소모성 아이템
    Weapon,     // 무기
    Armor,      // 방어구
    Quest,      // 퀘스트 아이템
    Other       // 기타
}

public class Item : ScriptableObject
{
    [SerializeField]
    private string itmeName;
    public string Name
    {
        get { return name; }
    }

    [SerializeField]
    private Type type;
    public Type ItemType
    {
        get { return type; }
    }

    [SerializeField]
    [TextArea]
    private string lores;
    public string Lores
    {
        get { return lores; }
    }

    public void OnUse(Entity target)
    {

    }
}