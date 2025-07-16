using UnityEngine;

public class OffWeaponInfo : EquipInfo
{
    protected override string GetTagName()
    {
        return "<º¸Á¶ ¹«±â Ä­>";
    }
    protected override void ShowEquips()
    {
        Debug.Log("Show Off Weapons");
    }
}