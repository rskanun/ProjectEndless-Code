using System.Collections.Generic;
using UnityEngine;

public class ActionManager : MonoBehaviour
{
    private enum SelectState
    {
        Action,
        Target,
        Turn
    }

    [Header("참조 스크립트")]
    [SerializeField] private ActionSelection actionSelection;
    [SerializeField] private TargetSelection targetSelection;
    [SerializeField] private TurnSelection turnSelection;

    // 참조 데이터
    private BattleData battleData;

    // 현재 선택창 정보
    private SelectState state;
    private int index;

    // 현재 턴 정보
    private BattleAction action;
    private Character actor;
    private List<Entity> targets;
    private float seqTurn;
    private int seqIndex;

    private void Awake()
    {
        battleData = BattleData.Instance;
    }

    public void SelectAction(Character actor)
    {
        this.actor = actor;

        // 행동 선택창 열기
        actionSelection.OpenSelection(actor);

        // 현재 상태 변경
        SetState(SelectState.Action);
    }

    private void SetState(SelectState state)
    {
        this.state = state;
    }

    public void UndoSelection()
    {
        switch (state)
        {
            case SelectState.Action:
                actionSelection.UndoSelection();
                break;

            case SelectState.Target:
                targetSelection.UndoSelection();
                actionSelection.ReopenSelection();
                break;

            case SelectState.Turn:
                turnSelection.UndoSelection();
                targetSelection.ReopenSelection();
                break;
        }
    }

    /***************************************************************
    * [ 행동 선택 ]
    * 
    * 현재 턴인 캐릭터가 어떤 행동을 취할 지 선택창 생성 및 처리
    ***************************************************************/

    public void OnSelectAttack()
    {
        AttackAction action = actor.CreateAttackAction();

        AttackType attackType = actor.AttackType;
        TargetType targetType = GetTargetType(attackType);

        OnSelectAction(action, targetType);
    }

    public void OnSelectSkill(Skill skill)
    {

    }

    public void OnSelectItem(Consumable item)
    {
        
    }

    public void OnSelectWaiting()
    {

    }

    public void OnSelectRun()
    {

    }

    private TargetType GetTargetType(AttackType attackType)
    {
        return (attackType == AttackType.Melee) ? TargetType.FrontEnemy : TargetType.Enemy;
    }

    private void OnSelectAction(BattleAction action, TargetType targetType)
    {
        this.action = action;

        // 행동 선택창 닫기
        actionSelection.CloseSelection();

        // 타겟 선택창 열기
        targetSelection.OpenSelection(targetType);

        // 현재 상태 변경
        SetState(SelectState.Target);
    }

    /***************************************************************
    * [ 타겟 선택 ]
    * 
    * 현재 행동의 타겟이 될 대상 선택
    ***************************************************************/

    public void SelectTarget(Entity target)
    {
        // 타겟 선택창 닫기
        targetSelection.CloseSelection();

        // 임시로 최소 턴 선택
        switch (action.actionType)
        {
            case ActionType.Attack:
                AttackAction attackAction = (AttackAction)action;
                attackAction.target = target;
                break;
        }

        // 턴 삽입
        PushActionData(action);
    }

    public void SelectTargets(List<Entity> targets)
    {

    }

    private void PushActionData(BattleAction action)
    {
        int minIndex = battleData.Sequence.GetMinIndex(action);

        actor.OnSelectAction(action, minIndex);
    }
}