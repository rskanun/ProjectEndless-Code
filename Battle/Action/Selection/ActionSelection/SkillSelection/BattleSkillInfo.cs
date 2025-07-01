using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BattleSkillInfo : MonoBehaviour, ISelectHandler
{
    [Header("스킬 정보 구성요소")]
    public Image icon;
    public Button button;
    public GameObject unusablePanel;
    public TextMeshProUGUI skillName;
    public TextMeshProUGUI costTurn;
    public TextMeshProUGUI costSP;

    // 스킬 정보
    private Skill skill;

    // 이벤트
    private Action hoverHandler;
    private Action clickHandler;

    public void SetSkill(Skill skill, Character caster)
    {
        this.skill = skill;

        // 정보 적용
        icon.sprite = skill.IconSprite;
        skillName.text = skill.Name;
        costTurn.text = caster.GetLastTurn(skill.CostTurn).ToString("0.0");
        costSP.text = $"{skill.CostSP} SP";
    }

    public Skill GetSkill()
    {
        return skill;
    }

    public void OnHover()
    {
        if (IsUsable())
        {
            hoverHandler?.Invoke();

            // 해당 오브젝트 선택
            EventSystem.current.SetSelectedGameObject(gameObject);
        }
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

    public void SetUsable(bool isUsed)
    {
        button.interactable = isUsed;
        unusablePanel.SetActive(!isUsed);
    }

    public bool IsUsable()
    {
        return button.interactable;
    }
}