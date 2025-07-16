using UnityEngine;

public class AccessoryInfo : EquipInfo
{
    protected override string GetTagName()
    {
        return "<¾Ç¼¼»ç¸® Ä­>";
    }

    protected override void ShowEquips()
    {
        Debug.Log("Show Accessories");
    }
}