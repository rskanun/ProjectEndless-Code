using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillInfo : MonoBehaviour, ISelectHandler
{
    [Header("스킬 정보 구성요소")]
    public Image icon;
    public TextMeshProUGUI skillName;
    public TextMeshProUGUI costTurn;
    public TextMeshProUGUI costSP;

    // 이벤트
    private Action hoverHandler;
    private Action clickHandler;

    public void SetSkill(Skill skill)
    {
        // 정보 적용
        icon.sprite = skill.IconSprite;
        skillName.text = skill.Name;
        costTurn.text = skill.CostTurn.ToString("0.0");
        costSP.text = $"{skill.CostSP} SP";
    }

    public void OnHover()
    {
        hoverHandler?.Invoke();

        // 해당 오브젝트 선택
        EventSystem.current.SetSelectedGameObject(gameObject);
    }

    public void SetHoverHandler(Action handler)
    {
        hoverHandler = handler;
    }

    public void OnClick()
    {
        clickHandler?.Invoke();
    }

    public void SetClickHandler(Action handler)
    {
        clickHandler = handler;
    }

    public void OnSelect(BaseEventData eventData)
    {
        hoverHandler?.Invoke();
    }
}