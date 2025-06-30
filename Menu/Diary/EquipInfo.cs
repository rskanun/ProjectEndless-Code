using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipInfo : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI nameField;
    [SerializeField] private TextMeshProUGUI addStatField;

    public void UpdateInfo(Weapon weapon)
    {
        nameField.text = weapon.Name;
        addStatField.text = AddStatToString(weapon);
    }

    private string AddStatToString(Weapon weapon)
    {
        List<string> addStats = new List<string>();

        if (weapon.STR != 0)
            addStats.Add($"STR {(weapon.STR > 0 ? "+" : "")}{weapon.STR}");

        if (weapon.AGI != 0)
            addStats.Add($"AGI {(weapon.AGI > 0 ? "+" : "")}{weapon.AGI}");

        return string.Join(" ¡¤ ", addStats);
    }
}