using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class TargetSelectButtonManager
{
    private static TargetSelectButtonManager _instance;
    public static TargetSelectButtonManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new TargetSelectButtonManager();

            return _instance;
        }
    }

    private HashSet<TargetSelectButton> buttons = new HashSet<TargetSelectButton>();
    private TargetSelectButton head;
    private TargetSelectButton tail;
    private Action buttonClickHandler;

    public void RegisterButton(TargetSelectButton button)
    {
        if (buttonClickHandler != null)
        {
            // 클릭 이벤트가 등록되어 있다면, 새로운 버튼이 등록되면 이벤트도 같이 등록
            button.AddListener(buttonClickHandler);
        }

        buttons.Add(button);
        AddButtonLinked(button);
    }

    public void RemoveButton(TargetSelectButton button)
    {
        buttons.Remove(button);
    }

    private void AddButtonLinked(TargetSelectButton button)
    {
        if (head == null && tail == null)
        {
            // 연결이 하나도 없는 경우 새 연결 생성
            head = button;
            tail = button;

            button.PrevButton = button;
            button.NextButton = button;
        }
        else
        {
            // 새 버튼은 항상 맨 뒤에 붙음
            button.PrevButton = tail;
            button.NextButton = head;

            tail.NextButton = button;
            head.PrevButton = button;

            tail = button;
        }
    }

    public void RegisterClickHandler(Action handler)
    {
        buttonClickHandler = handler;

        foreach (TargetSelectButton button in buttons)
        {
            button.AddListener(handler);
        }
    }

    public void ActiveButtons(Func<Entity, bool> activeCondition)
    {
        TargetSelectButton firstSelectButton = null;

        // 특정 버튼만 활성화
        foreach (TargetSelectButton button in buttons)
        {
            Entity target = button.target;

            // 조건에 맞는 엔티티의 버튼만 활성화
            button.interactable = activeCondition(target);

            // 활성화된 버튼이 없을 경우
            if (firstSelectButton == null && button.interactable)
            {
                // 임시로 첫번째 버튼 저장
                firstSelectButton = button;
            }
        }

        // 이전 버튼 선택
        if (TargetSelectButton.lastSelected == null || TargetSelectButton.lastSelected.interactable == false)
        {
            // 이전에 선택한 버튼을 선택할 수 없는 경우 선택가능한 첫 버튼 선택
            TargetSelectButton.lastSelected = firstSelectButton;
        }

        EventSystem.current.SetSelectedGameObject(TargetSelectButton.lastSelected.gameObject);
    }

    public void DeactiveAllButtons()
    {
        // 모든 버튼 비활성화
        foreach (TargetSelectButton button in buttons)
        {
            if (button != null)

                button.interactable = false;

            // 멀티 선택된 버튼도 전부 초기화
            button.DeselectedMultiButton();
        }

        // 선택 버튼 초기화
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void SelectButtons(Func<Entity, bool> selectCondition)
    {
        TargetSelectButton firstSelectButton = null;

        // 특정 버튼만 활성화
        foreach (TargetSelectButton button in buttons)
        {
            Entity target = button.target;

            // 살아있는 엔티티 중 조건에 맞는 엔티티의 버튼만 활성화 및 선택
            button.interactable = selectCondition(target);

            if (button.interactable)
            {
                button.MultiSelected();

                if (firstSelectButton == null)
                {
                    firstSelectButton = button;
                }
            }
        }

        // 활성화 된 버튼 중 아무(첫번째) 버튼 선택
        EventSystem.current.SetSelectedGameObject(firstSelectButton.gameObject);
    }

    public List<Entity> GetSelectedTargets()
    {
        return buttons.Where(btn => btn.IsSelected)
            .Select(btn => btn.target)
            .ToList();
    }
}