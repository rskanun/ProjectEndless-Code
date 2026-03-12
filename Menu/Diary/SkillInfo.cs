using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillInfo : DiaryInfo, ISubmitHandler
{
    private static SkillInfo prevSkillInfo;

    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI nameField;
    [SerializeField] private TextMeshProUGUI costField;
    [SerializeField] private TextMeshProUGUI turnField;

    private Skill skill;
    private bool isShowInfo;

    public void UpdateInfo(Skill skill)
    {
        this.skill = skill;

        nameField.text = skill.Name;
        costField.text = $"{skill.CostSP} SP";
        turnField.text = skill.CostTurn.ToString("0.0");
    }

    public void OnSubmit(BaseEventData eventData)
    {
        SubmitHandler();
    }

    public void OnClick()
    {
        SubmitHandler();
    }

    protected override void DeselectHandler()
    {
        isShowInfo = false;
    }

    protected override void SelectHandler()
    {
        if (!diary.IsFocusToSkill) return;

        // 스킬에 초점이 둬진 상태라면 정보 보이기
        SubmitHandler();
    }

    private void SubmitHandler()
    {
        // 해당 스킬의 정보를 보이고 있는 경우 무시
        if (isShowInfo) return;

        // true -> down / false -> up
        bool direction = (prevSkillInfo == null) ? false
            : prevSkillInfo.transform.position.y < transform.position.y;

        // 정보 띄우기
        if (diary.IsFocusToSkill) app.SwapSkillInformation(skill, direction);
        else app.ShowSkillInformation(skill);

        isShowInfo = true;
        diary.IsFocusToSkill = true;

        prevSkillInfo = this;
    }
}