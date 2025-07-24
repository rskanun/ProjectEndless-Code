public class AccessoryContactWindow : EquipContactWindow
{
    public override ItemType EquipType => ItemType.Accessory;

    protected override bool IsEquipType(CharacterData character, Equip equip)
    {
        return equip as Accessory;
    }

    protected override void EquipItem(CharacterData character, Equip selectItem)
    {
        if (selectItem is not Accessory accessory) return;

        if (app.State == ContactState.Accessory1)
            character.Accessory1 = accessory;
        else
            character.Accessory2 = accessory;
    }

    protected override bool IsEquip(CharacterData chr, Equip accessory)
    {
        if (app.State == ContactState.Accessory1)
            return chr.Accessory1 == accessory;
        else
            return chr.Accessory2 == accessory;
    }
}