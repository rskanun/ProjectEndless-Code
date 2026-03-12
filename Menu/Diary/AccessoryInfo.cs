using UnityEngine;

public class AccessoryInfo : EquipInfo
{
    private enum AccessorySlot
    {
        Slot1,
        Slot2,
    }

    [Header("악세사리 장착 슬롯")]
    [SerializeField] private AccessorySlot slot;

    protected override string GetTagName()
    {
        return "<악세사리 칸>";
    }

    protected override void ShowEquips()
    {
        if (slot == AccessorySlot.Slot1) app.ShowSlot1Accessory();
        else app.ShowSlot2Accessory();
    }
}