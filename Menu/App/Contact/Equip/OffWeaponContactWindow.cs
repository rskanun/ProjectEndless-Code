public class OffWeaponContactWindow : WeaponContactWindow
{
    public override WeaponType ShowWeaponType => WeaponType.Off;

    protected override void EquipItem(CharacterData character, Equip weapon)
    {
        if (weapon is not Weapon) return;

        character.OffWeapon = (Weapon)weapon;
    }

    protected override bool IsEquip(CharacterData chr, Equip weapon)
    {
        return chr.OffWeapon == weapon;
    }
}