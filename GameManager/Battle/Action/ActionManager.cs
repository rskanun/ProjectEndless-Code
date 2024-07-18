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
    private Dictionary<ActionType, ActionHandler> actionHandlers;

    // 현재 턴 정보
    private Character actor;
    private Entity target;
    private int seqIndex = -1;

    private void Awake()
    {
        actionHandlers = new Dictionary<ActionType, ActionHandler>();

        // 각 행동 타입별 실행자의 행동 삽입
        actionHandlers.Add(ActionType.Attack, (target) => actor.OnAttack(target));
    }

    public void SelectAction(Character actor)
    {
        this.actor = actor;
        target = null;
        seqIndex = -1;

        // 행동 선택창 열기
        ui.OpenSelectionWindow();
    }

    public void OnSelectAttack()
    {
        StartCoroutine(SetupAction(ActionType.Attack, () => selectionManager.SelectFront()));
    }

    private IEnumerator SetupAction(ActionType action, Action selectionAction)
    {
        // 선택창 닫기
        ui.CloseSelectionWindow();

        // 대상 선택 UI가 필요하다면 활성화
        if (selectionAction != null)
        {
            selectionAction.Invoke();

            // 타겟 선택이 완료되었다면 다음 단계로 진행
            yield return new WaitUntil(() => target != null);
        }

        // 턴 수 선택
        yield return new WaitUntil(() => seqIndex >= 0);

        // 행동 실행
        actionHandlers[action].Invoke(target);
    }

    public void SelectTarget(Entity target)
    {
        this.target = target;
    }

    public void SetTurn(int index)
    {

    }
}