public class OffWeaponContactWindow : WeaponContactWindow
{
    protected override bool IsEquipType(Weapon weapon)
    {
        return weapon.IsOffType;
    }
}