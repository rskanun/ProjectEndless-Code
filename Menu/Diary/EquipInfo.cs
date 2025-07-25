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
        // 사용할 수 없는 칸이면 장비 및 교체 X
        if (!isAvailable) return;

        // 장비 목록 띄우기
        ShowEquips();
    }

    protected override void SelectHandler()
    {
        if (!diary.IsFocusToSkill) return;

        // 장비 선택 시엔 스킬 초점 해제
        diary.IsFocusToSkill = false;

        // 스킬 정보창 닫기
        app.HideSkillInformation();
    }

    protected abstract string GetTagName();
    protected abstract void ShowEquips();
}