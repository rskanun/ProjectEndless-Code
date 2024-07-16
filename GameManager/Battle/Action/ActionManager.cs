using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManager : MonoBehaviour
{
    [Header("참조 스크립트")]
    [SerializeField] private ActionUI ui;
    [SerializeField] private SelectionManager selectionManager;

    private delegate void ActionHandler(Entity target);
    private Dictionary<ActionType, ActionHandler> actionHandlers = new();
    private ActionHandler onTargetSelected;

    // 현재 턴인 캐릭터
    private Character actor;

    private void Awake()
    {
        actionHandlers.Add(ActionType.Attack, (target) => actor.OnAttackAction(target));
    }

    public void OnSelectAction(Character actor)
    {
        this.actor = actor;

        // 행동 선택창 열기
        ui.ActiveSelection(true);
    }

    public void OnSelectAttack()
    {
        SetupAction(ActionType.Attack, () => selectionManager.SelectFront());
    }

    private void SetupAction(ActionType action, Action selectionAction)
    {
        // 선택창 닫기
        ui.ActiveSelection(false);

        // 대상 선택 UI가 필요하다면 활성화
        selectionAction?.Invoke();

        // 행동 예약
        if (actionHandlers.TryGetValue(action, out ActionHandler handler))
        {
            onTargetSelected = handler;
        }
    }

    public void SelectTarget(GameObject selectObj)
    {
        Entity target = selectObj.GetComponent<Entity>();

        onTargetSelected?.Invoke(target);
    }
}