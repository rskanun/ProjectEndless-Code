using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipInfo : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI nameField;
    [SerializeField] private string tagName;

    public void UpdateInfo(Equip equip)
    {
        nameField.text = equip != null ? equip.Name : tagName;
    }

    private string AddStatToString(Equip equip)
    {
        List<string> addStats = new List<string>();

        if (equip.STR != 0)
            addStats.Add($"STR {(equip.STR > 0 ? "+" : "")}{equip.STR}");

        if (equip.DEF != 0)
            addStats.Add($"DEF {(equip.DEF > 0 ? "+" : "")}{equip.DEF}");

        if (equip.AGI != 0)
            addStats.Add($"AGI {(equip.AGI > 0 ? "+" : "")}{equip.AGI}");

        if (equip.DEX != 0)
            addStats.Add($"DEX {(equip.DEX > 0 ? "+" : "")}{equip.DEX}");

        return string.Join(" · ", addStats);
    }
}