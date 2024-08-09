using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TargetSelectButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, ISubmitHandler, ISelectHandler, IDeselectHandler, IMoveHandler
{
    public static TargetSelectButton lastSelected;

    public bool interactable;
    private bool isSelected;

    [HideInInspector]
    public Entity targetEntity;
    public Image targetGraphic;
    public Sprite selectedSprite;
    private Sprite originSprite;

    private TargetSelectButton _prevButton;
    public TargetSelectButton PrevButton
    {
        set { _prevButton = value; }
        get
        {
            // 이전 버튼이 선택 가능한 버튼인 것만 리턴
            TargetSelectButton button = _prevButton;

            while(button != this && !button.interactable)
            {
                button = button.PrevButton;
            }

            return button;
        }
    }
    private TargetSelectButton _nextButton;
    public TargetSelectButton NextButton
    {
        set { _nextButton = value; }
        get
        {
            // 다음 버튼이 선택 가능한 버튼인 것만 리턴
            TargetSelectButton button = _nextButton;

            while (button != this && !button.interactable)
            {
                button = button.NextButton;
            }

            return button;
        }
    }

    [SerializeField]
    private UnityEvent onClick;

    private void Awake()
    {
        originSprite = targetGraphic.sprite;
    }

    public void OnSubmit(BaseEventData eventData)
    {
        if (EventSystem.current.currentSelectedGameObject == gameObject)
        {
            // 선택된 상태인 경우 클릭 시 이벤트 실행
            OnClick();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick();
    }

    public void OnClick()
    {
        if (interactable)
        {
            lastSelected = this;

            onClick?.Invoke();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (interactable)
        {
            Selected();
        }
    }

    public void AddListener(UnityAction listener)
    {
        onClick.AddListener(() => listener.Invoke());
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (interactable)
        {
            Selected();
        }
    }

    public void Selected()
    {
        // 이미 선택된 경우 무시
        if (isSelected) return;

        // 선택된 버튼이 이것과 다른 경우
        GameObject selectObj = EventSystem.current.currentSelectedGameObject;
        TargetSelectButton selectedButton = selectObj?.GetComponent<TargetSelectButton>();
        if (selectedButton == null || selectedButton != this)
        {
            // 선택된 버튼이 있는 경우 해당 버튼 해제
            if (selectedButton != null)
            {
                selectedButton.Deselected();
            }

            // 해당 버튼을 선택된 버튼으로 선택
            SelectionData.SetSelectedObject(gameObject);
        }

        // 버튼 선택
        SelectedButton();
    }

    public void MultiSelected()
    {
        // 이미 선택된 경우 무시
        if (isSelected) return;

        SelectedButton();
    }

    private void SelectedButton()
    {
        isSelected = true;

        targetGraphic.sprite = selectedSprite;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        Deselected();
    }

    public void Deselected()
    {
        // 선택 해제
        isSelected = false;

        // 그래픽 변경
        targetGraphic.sprite = originSprite;
    }

    public void OnMove(AxisEventData eventData)
    {
        if (eventData.moveDir == MoveDirection.Left)
        {
            PrevButton.Selected();
        }
        else if (eventData.moveDir == MoveDirection.Right)
        {
            NextButton.Selected();
        }
    }
}