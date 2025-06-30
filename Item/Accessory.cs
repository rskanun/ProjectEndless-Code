using UnityEngine;

[CreateAssetMenu(fileName = "Accessory", menuName = "Item/Accessory")]
public class Accessory : Item
{
    public override ItemType Type => ItemType.Accessory;

    [Header("악세서리 아이템 정보")]
    [SerializeField]
    private int _defensive;
    public int DEF => _defensive;

    [SerializeField]
    private int _agility;
    public int AGI => _agility;

    [SerializeField]
    private int _dexterity;
    public int DEX => _dexterity;

    [SerializeField]
    private Skill _skill;
    public Skill Skill => _skill;
}