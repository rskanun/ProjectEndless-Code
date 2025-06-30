using UnityEngine;

public enum WeaponType
{
    BastardSword,
    Dagger
}

[CreateAssetMenu(fileName = "Weapon", menuName = "Item/Weapon")]
public class Weapon : Equip
{
    public override ItemType Type => ItemType.Weapon;

    [Header("무기 아이템 정보")]
    [SerializeField]
    private WeaponType _weaponType;
    public WeaponType WeaponType => _weaponType;
}