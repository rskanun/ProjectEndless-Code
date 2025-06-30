using UnityEngine;

public enum WeaponType
{
    BastardSword,
    Dagger
}

[CreateAssetMenu(fileName = "Weapon", menuName = "Item/Weapon")]
public class Weapon : Item
{
    public override ItemType Type => ItemType.Weapon;

    [Header("무기 아이템 정보")]
    [SerializeField]
    private WeaponType _weaponType;
    public WeaponType WeaponType => _weaponType;

    [SerializeField]
    private int _strength;
    public int STR => _strength;

    [SerializeField]
    private int _agility;
    public int AGI => _agility;

    [SerializeField]
    private Skill _skill;
    public Skill Skill => _skill;
}