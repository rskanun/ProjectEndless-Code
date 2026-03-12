using System.Collections.Generic;
using UnityEngine;

[System.Flags]
public enum WeaponType
{
    None = 0,

    // 메인
    BastardSword = 1 << 0,
    Katana = 1 << 1,
    Main = BastardSword | Katana,

    // 보조
    Shield = 1 << 5,
    Off = Shield,

    // 둘다
    Dagger = 1 << 10,
    Gun = 1 << 11,
    Both = Dagger | Gun
}

public static class WeaponHelper
{
    private static HashSet<WeaponType> twoHand = new()
    {
        WeaponType.BastardSword, WeaponType.Katana
    };

    public static bool IsTwoHand(this WeaponType type)
    {
        return twoHand.Contains(type);
    }
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