using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class EquipInfo : MonoBehaviour, ISelectHandler, IDeselectHandler, ISubmitHandler
{
    [SerializeField] protected ContactApp app;
    [SerializeField] protected Diary diary;
    [Space]
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI nameField;
    [SerializeField] private Image selectMark;

    private bool isModifyMode;

    public void UpdateInfo(Equip equip)
    {
        nameField.text = equip != null ? equip.Name : GetTagName();
    }

    public void OnSelect(BaseEventData eventData)
    {
        // 변경 모드에서 선택된 거라면 모드만 해제
        if (isModifyMode) isModifyMode = false;
        else ActiveSelectMark();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        // 해당 칸의 장비를 변경 중이라면 선택 표식 해제 X
        if (isModifyMode) return;

        DeactiveSelectMark();
    }

    public void OnSubmit(BaseEventData eventData)
    {
        SubmitHandler();
    }

    public void OnClick()
    {
        // select -> submit와 같은 순서로 진행
        ActiveSelectMark();
        SubmitHandler();
    }

    private void ActiveSelectMark()
    {
        selectMark.gameObject.SetActive(true);
        selectMark.DOFade(0.25f, 0.5f).SetLoops(-1, LoopType.Yoyo);
    }

    private void DeactiveSelectMark()
    {
        selectMark.DOKill();

        // 원래 알파값으로 되돌리기
        Color color = selectMark.color;
        color.a = 0.0f;
        selectMark.color = color;

        selectMark.gameObject.SetActive(false);
    }

    private void SubmitHandler()
    {
        여기 문제

        // 해당 장비칸이 선택 되었다면 변경 모드로 들어가기
        isModifyMode = true;

        // 마지막 선택 버튼으로 설정
        diary.SetLastSelectedButton(gameObject);

        // 장비 목록 띄우기
        ShowEquips();
    }

    protected abstract string GetTagName();
    protected abstract void ShowEquips();
}