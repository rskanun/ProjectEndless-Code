using UnityEngine;

public abstract class Equip : Item
{
    [Header("아이템 능력치")]
    [SerializeField]
    private int _strength;
    public int STR => _strength;

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