using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipInfo : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI nameField;
    [SerializeField] private TextMeshProUGUI addStatField;
    [SerializeField] private string tagName;

    public void UpdateInfo(Equip equip)
    {
        // ?? ??? ??? ?? ?? ?? ?? ??? ? ??? ??
        if (equip == null)
        {
            nameField.text = tagName;
            addStatField.text = "";
            return;
        }

        nameField.text = equip.Name;
        addStatField.text = AddStatToString(equip);
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