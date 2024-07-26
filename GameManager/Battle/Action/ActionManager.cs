using System.Collections.Generic;
using UnityEngine;

public class ActionManager : MonoBehaviour
{
    [Header("참조 스크립트")]
    [SerializeField] private ActionSelection actionSelection;
    [SerializeField] private TargetSelection targetSelection;
    [SerializeField] private TurnSelection turnSelection;

    // 참조 데이터
    private BattleData battleData;

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
    }

    public void UndoAction()
    {
        actionSelection.ReopenSelection();
    }

    public void UndoTarget()
    {
        actionSelection.ReopenSelection();
    }

    public void UndoTurn()
    {
        targetSelection.ReopenSelection();
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

        switch (action.actionType)
        {
            case ActionType.Attack:
                AttackAction attackAction = (AttackAction)action;
                attackAction.target = target;
                break;
        }

        // 턴 선택창 열기
        turnSelection.OpenSelection(action);
    }

    public void SelectTargets(List<Entity> targets)
    {

    }

    /***************************************************************
    * [ 턴 선택 ]
    * 
    * 현재 행동이 배치될 턴 설정
    ***************************************************************/

    private void PushActionData(BattleAction action)
    {
        int minIndex = battleData.Sequence.GetMinIndex(action);

        actor.OnSelectAction(action, minIndex);
    }
}