public class WeaponContactWindow : EquipContactWindow
{
    public override ItemType EquipType => ItemType.Weapon;
    public virtual WeaponType ShowWeaponType => WeaponType.Main;

    protected override bool IsEquipType(CharacterData character, Equip equip)
    {
        if (equip is not Weapon weapon) return false;

        // 아래의 조건을 만족하는 무기 타입인지 리턴
        // 1. 해당 창에서 띄울 타입 혹은 둘 다 낄 수 있는 무기 타입인지
        // 2. 플레이어가 착용할 수 있는 타입인지
        return ((ShowWeaponType | WeaponType.Both) & character.UsableWeaponType & weapon.WeaponType) != 0;
    }

    protected override void EquipItem(CharacterData character, Equip weapon)
    {
        if (weapon is not Weapon) return;

        character.MainWeapon = (Weapon)weapon;
    }

    protected override bool IsEquip(CharacterData chr, Equip weapon)
    {
        return chr.MainWeapon == weapon;
    }
}