using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TargetSelectButton : MonoBehaviour, IPointerEnterHandler
{
    public static bool multiSelectEnabled;
    public static TargetSelectButton selectedButton;

    public bool interactable;
    private bool isSelected;

    [HideInInspector]
    public Entity targetEntity;
    public Image targetGraphic;
    public Sprite selectedSprite;

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

    public void OnClick()
    {
        if (interactable)
        {
            onClick?.Invoke();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (interactable)
        {
            OnSelected();
        }
    }

    public void AddListener(UnityAction listener)
    {
        onClick.AddListener(() => listener.Invoke());
    }

    public void SetSelected(bool isSelected)
    {
        if (isSelected) OnSelected();
        else OnDeselected();
    }

    private void OnSelected()
    {
        // 이미 선택된 경우 무시
        if (isSelected) return;

        isSelected = true;

        // 다중 선택이 가능한 경우가 아닐 경우
        if (multiSelectEnabled == false)
        {
            // 이전 버튼 선택 해제
            selectedButton.OnDeselected();

            // 선택된 버튼 변경
            selectedButton = this;
        }

        targetGraphic.sprite = selectedSprite;
    }

    private void OnDeselected()
    {
        // 선택 해제
        isSelected = false;

        if (selectedButton == this)
        {
            selectedButton = null;
        }

        // 그래픽 변경
        targetGraphic.sprite = null;
    }
}