using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class EquipInfo : DiaryInfo, ISubmitHandler
{
    [SerializeField] protected Image icon;
    [SerializeField] protected TextMeshProUGUI nameField;

    protected bool isAvailable = true;

    public virtual void UpdateInfo(Equip equip)
    {
        nameField.text = equip != null ? equip.Name : GetTagName();
    }

    public void OnSubmit(BaseEventData eventData)
    {
        SubmitHandler();
    }

    public void OnClick()
    {
        SubmitHandler();
    }

    private void SubmitHandler()
    {
        // ?ъ⑺ ? ?? 移몄대㈃ ?λ? 諛 援泥?X
        if (!isAvailable) return;

        // ?λ? 紐⑸? ??곌린
        ShowEquips();
    }

    protected override void SelectHandler()
    {
        if (!diary.IsFocusToSkill) return;

        // ?λ? ?? ?? ?ㅽ?珥? ?댁
        diary.IsFocusToSkill = false;

        // ?ㅽ??蹂댁갹 ?リ린
        app.HideSkillInformation();
    }

    protected abstract string GetTagName();
    protected abstract void ShowEquips();
}