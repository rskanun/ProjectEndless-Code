using UnityEngine;

public class AccessoryInfo : EquipInfo
{
    private enum AccessorySlot
    {
        Slot1,
        Slot2,
    }

    [Header("¾Ç¼¼»ç¸® ÀåÂø ½½·Ô")]
    [SerializeField] private AccessorySlot slot;

    protected override string GetTagName()
    {
        return "<¾Ç¼¼»ç¸® Ä­>";
    }

    protected override void ShowEquips()
    {
        if (slot == AccessorySlot.Slot1) app.ShowSlot1Accessory();
        else app.ShowSlot2Accessory();
    }
}