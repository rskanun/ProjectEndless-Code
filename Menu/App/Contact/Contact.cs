using System;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class Contact : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerClickHandler
{

    private Action clickHandler;
    private Action selectHandler;

    private void OnEnable()
    {
        // 해당 오브젝트가 선택된 경우
        if (EventSystem.current.currentSelectedGameObject == gameObject)
        {
            // 핸들러 작동
            SelectHandler();
        }
    }

    public void SetClickHandler(Action handler)
    {
        clickHandler = handler;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        clickHandler?.Invoke();
    }

    public void SetSelectAction(Action handler)
    {
        selectHandler = handler;
    }

    public virtual void OnSelect(BaseEventData eventData)
    {
        SelectHandler();
    }

    protected virtual void SelectHandler()
    {
        selectHandler?.Invoke();
    }

    public virtual void OnDeselect(BaseEventData eventData)
    {

    }
}