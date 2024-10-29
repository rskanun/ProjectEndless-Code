using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TargetSelectButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, ISubmitHandler, ISelectHandler, IDeselectHandler, IMoveHandler
{
    public static TargetSelectButton lastSelected;

    public bool interactable;
    private bool isMultiSelected;
    private bool isSelected;
    public bool IsSelected
    {
        get { return isSelected; }
    }

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

            while (button != this && !button.interactable)
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

    private void OnClick()
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
            AutoSelector.SetSelectedObject(gameObject);
        }

        // 버튼 선택
        SelectedButton();

        // 예상 체력 보여주기
        ForecastHP(CurrentBattleData.Instance.SelectionData.action);
    }

    public void MultiSelected()
    {
        // 이미 선택된 경우 무시
        if (isSelected) return;

        // 멀티로 버튼 선택
        isMultiSelected = true;
        SelectedButton();

        // 예상 체력 보여주기
        ForecastHP(CurrentBattleData.Instance.SelectionData.action);
    }

    private void SelectedButton()
    {
        isSelected = true;

        targetGraphic.sprite = selectedSprite;
    }

    private void ForecastHP(BattleAction action)
    {
        float attackDmg = GetAttackDmg(action);
        int lastDmg = targetEntity.GetLastDmg(attackDmg);

        targetEntity.SetForecastHP(-lastDmg);
    }

    private float GetAttackDmg(BattleAction action)
    {
        if (action is AttackAction)
        {
            // 일반 공격은 해당 캐릭터의 자체 데미지 가져오기
            return action.actor.AttackDmg;
        }
        else if (action is SkillAction)
        {
            SkillAction skillAction = (SkillAction)action;
            AttackSkill skill = skillAction.castSkill as AttackSkill;

            if (skill != null)
            {
                // 공격 스킬만 데미지 계산
                return skill.GetSkillDmg(action.actor);
            }
        }

        // 나머지 행동은 데미지 X
        return 0.0f;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        Deselected();
    }

    public void Deselected()
    {
        if (isMultiSelected)
        {
            // 멀티 선택 중이라면 선택해제 X
            return;
        }

        // 선택 해제
        isSelected = false;

        // 그래픽 변경
        targetGraphic.sprite = originSprite;

        // 예상 체력바 비활성화
        targetEntity.SetActiveForecastHP(false);
    }

    public void DeselectedMultiButton()
    {
        // 멀티 선택 해제
        isMultiSelected = false;

        // 기존 선택 해제 실행
        Deselected();
    }

    public void OnMove(AxisEventData eventData)
    {
        if (eventData.moveDir == MoveDirection.Left)
        {
            AutoSelector.SetSelectedObject(PrevButton.gameObject);
        }
        else if (eventData.moveDir == MoveDirection.Right)
        {
            AutoSelector.SetSelectedObject(NextButton.gameObject);
        }
    }
}