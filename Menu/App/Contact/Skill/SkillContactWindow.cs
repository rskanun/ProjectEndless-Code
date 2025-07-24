using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillInfomationWindow : MonoBehaviour
{
    [SerializeField] private Diary diary;
    [Space]
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI nameField;
    [SerializeField] private TextMeshProUGUI typeField;
    [SerializeField] private TextMeshProUGUI infoField;
    [SerializeField] private TextMeshProUGUI descriptionField;


    public void ShowInfo(Skill skill)
    {

    }

    private void SetupInformation(Skill skill)
    {
        icon.sprite = skill.IconSprite;
        nameField.text = skill.Name;
        typeField.text = skill.GetTypeName();
        infoField.text = GetSkillInfoStr(skill);
        descriptionField.text = skill.Description;
    }

    private string GetSkillInfoStr(Skill skill)
    {
        var sb = new System.Text.StringBuilder();

        sb.AppendLine($"소모 기력: {skill.CostSP}");
        sb.AppendLine($"사용 턴: {skill.CostTurn}");
        sb.Append($"타격 범위: {skill.TargetType.GetTypeName()}");

        return sb.ToString();
    }
}