using UnityEngine;

public class ActionManager : MonoBehaviour
{
    [Header("참조 스크립트")]
    [SerializeField] private ActionUI ui;
    [SerializeField] private SelectionManager selectionManager;

    private delegate void ActionHandler(Entity target);
    private ActionHandler onTargetSelected;

    // 현재 턴인 캐릭터
    private Character actor;

    public void OnSelectAction(Character actor)
    {
        this.actor = actor;

        // 행동 선택창 열기
        ui.ActiveSelection(true);
    }

    public void OnSelectAttack()
    {
        ui.ActiveSelection(false);

        // 공격 대상 선택 UI 활성화
        selectionManager.NotifySelectableFront();

        // 공격 대상 선택 시 행동 예약
        onTargetSelected = (target) => actor.OnAttackAction(target);
    }

    public void SetTarget()
    {
        GameObject selectObj = selectionManager.SelectTarget;
        Entity target = selectObj.GetComponent<Entity>();

        onTargetSelected(target);
    }
}