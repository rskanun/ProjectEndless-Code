using UnityEngine;

public class ActionManager : MonoBehaviour
{
    [Header("참조 스크립트")]
    [SerializeField] private ActionUI ui;

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
        ui.OnSelectEnemy();

        // 공격 대상 선택 시 행동 예약
        onTargetSelected = (target) =>
        {
            float turn = 1.0f;  // 임시 턴수

            AttackAction action = new AttackAction();

            action.remainTurn = turn;
            action.attacker = actor;
            action.target = target;

            actor.OnAttackAction(action);
        };
    }

    public void SetTarget(Entity target)
    {
        ui.CloseTargetSelection();
        onTargetSelected(target);
    }
}