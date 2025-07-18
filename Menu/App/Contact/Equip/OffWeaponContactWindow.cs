public class OffWeaponContactWindow : WeaponContactWindow
{
    public override WeaponType ShowType => WeaponType.Off;

    protected override void EquipItem(CharacterData character, Weapon selectWeapon)
    {
        character.OffWeapon = selectWeapon;

        // 장비 교체 후 알림
        GameEventManager.Instance.NotifyEquipUpdate();
    }

    protected override bool IsEquip(CharacterData chr, Weapon weapon)
    {
        return chr.OffWeapon == weapon;
    }
}