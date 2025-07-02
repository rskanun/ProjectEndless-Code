using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class Contact : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] protected Image selectMark;

    private Action selectHandler;
    private Tween selectionTween;

    private void OnEnable()
    {
        // 해당 오브젝트가 선택된 경우
        if (EventSystem.current.currentSelectedGameObject == gameObject)
        {
            // 핸들러 작동
            SelectHandler();
        }
    }

    private void OnDisable()
    {
        // 선택이 해제되지 않고 비활성화 될 경우 대비
        DeselectHandler();
    }

    public void SetSelectAction(Action handler)
    {
        selectHandler = handler;
    }

    public void OnSelect(BaseEventData eventData)
    {
        SelectHandler();
    }

    private void SelectHandler()
    {
        selectHandler?.Invoke();
        selectMark.gameObject.SetActive(true);

        // 선택 애니메이션
        selectionTween = selectMark.DOFade(0.25f, 0.5f)
            .SetLoops(-1, LoopType.Yoyo);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        DeselectHandler();
    }

    private void DeselectHandler()
    {
        // 비활성화 전 애니메이션 삭제
        selectionTween.Kill();

        selectMark.color = new Color(selectMark.color.r, selectMark.color.g, selectMark.color.b, 0f);
        selectMark.gameObject.SetActive(false);
    }
}