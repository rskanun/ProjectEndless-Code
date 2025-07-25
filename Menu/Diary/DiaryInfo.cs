using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class DiaryInfo : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] protected ContactApp app;
    [SerializeField] protected Diary diary;
    [Space]
    [SerializeField] protected Image selectMark;

    private bool isSelect;

    private void OnDisable()
    {
        DeactiveSelectMark();
    }

    public void OnSelect(BaseEventData eventData)
    {
        // 현재 선택된 상태라면 재선택 X
        if (isSelect) return;

        // 선택 마크 활성화
        ActiveSelectMark();

        SelectHandler();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        DeselectHandler();

        if (app.State != ContactState.Party || !isSelect) return;

        // 파티 메뉴로 돌아왔으면 선택 제거
        DeactiveSelectMark();

        // 마지막 선택 정보 제거
        diary.SetLastSelectedButton(null);
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

    protected virtual void SelectHandler() { }
    protected virtual void DeselectHandler() { }
}