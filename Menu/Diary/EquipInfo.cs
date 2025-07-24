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
    [SerializeField] protected Image icon;
    [SerializeField] protected TextMeshProUGUI nameField;
    [SerializeField] protected Image selectMark;

    protected bool isAvailable = true;
    private bool isSelect;

    private void OnDisable()
    {
        DeactiveSelectMark();
    }

    public virtual void UpdateInfo(Equip equip)
    {
        nameField.text = equip != null ? equip.Name : GetTagName();
    }

    public void OnSelect(BaseEventData eventData)
    {
        // 현재 선택된 상태라면 재선택 X
        if (isSelect) return;

        ActiveSelectMark();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (app.State != ContactState.Party || !isSelect) return;

        // 파티 메뉴로 돌아왔으면 선택 제거
        DeactiveSelectMark();

        // 마지막 선택 정보 제거
        diary.SetLastSelectedButton(null);
    }

    public void OnSubmit(BaseEventData eventData)
    {
        SubmitHandler();
    }

    public void OnClick()
    {
        SubmitHandler();
    }

    private void ActiveSelectMark()
    {
        isSelect = true;

        // 이전 버튼 선택마크 비활성화
        diary.LastSelectedInfo?.DeactiveSelectMark();

        // 해당 정보칸을 마지막 선택 정보로 설정
        diary.SetLastSelectedButton(this);

        // 선택 이펙트 활성화
        selectMark.gameObject.SetActive(true);
        selectMark.DOFade(0.25f, 0.5f).SetLoops(-1, LoopType.Yoyo);
    }

    private void DeactiveSelectMark()
    {
        isSelect = false;
        selectMark.DOKill();

        // 원래 알파값으로 되돌리기
        Color color = selectMark.color;
        color.a = 0.0f;
        selectMark.color = color;

        selectMark.gameObject.SetActive(false);
    }

    private void SubmitHandler()
    {
        // 사용할 수 없는 칸이면 장비 및 교체 X
        if (!isAvailable) return;

        // 장비 목록 띄우기
        ShowEquips();
    }

    protected abstract string GetTagName();
    protected abstract void ShowEquips();
}