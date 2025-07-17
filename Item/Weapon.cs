using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    BastardSword,
    Dagger
}

public static class WeaponHelper
{
    private static readonly HashSet<WeaponType> MainTypes = new()
    {
        WeaponType.BastardSword,
        WeaponType.Dagger,
    };

    private static readonly HashSet<WeaponType> OffTypes = new()
    {
        WeaponType.Dagger,
    };

    public static bool IsMain(this WeaponType type)
    {
        return MainTypes.Contains(type);
    }

    public static bool IsOff(this WeaponType type)
    {
        return OffTypes.Contains(type);
    }
}

[CreateAssetMenu(fileName = "Weapon", menuName = "Item/Weapon")]
public class Weapon : Equip
{
    public override ItemType Type => ItemType.Weapon;
    public bool IsMainType => WeaponType.IsMain();
    public bool IsOffType => WeaponType.IsOff();

    [Header("무기 아이템 정보")]
    [SerializeField]
    private WeaponType _weaponType;
    public WeaponType WeaponType => _weaponType;
}