using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillInfo : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI nameField;
    [SerializeField] private TextMeshProUGUI costField;
    [SerializeField] private TextMeshProUGUI turnField;

    public void UpdateInfo(Skill skill)
    {
        nameField.text = skill.Name;
        costField.text = $"{skill.CostSP} SP";
        turnField.text = skill.CostTurn.ToString("0.0");
    }
}