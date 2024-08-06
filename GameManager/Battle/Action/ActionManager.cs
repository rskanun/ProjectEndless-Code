using System.Collections.Generic;
using UnityEngine;

public class ActionManager : MonoBehaviour
{
    [Header("참조 스크립트")]
    [SerializeField] private ActionSelection actionSelection;

    // 참조 데이터
    private BattleData battleData;

    // 현재 열린 창
    private Stack<ISelection> selectionLog;

    // 현재 턴 정보
    private List<Entity> targets;
    private float seqTurn;
    private int seqIndex;

    private BattleAction action;
    private Character actor;

    private void Awake()
    {
        battleData = BattleData.Instance;
    }

    public Character GetActor()
    {
        return actor;
    }

    public BattleAction GetAction()
    {
        return action;
    }

    public void OpenActionSelection(Character actor)
    {
        this.actor = actor;

        // 로그 초기화
        selectionLog = new Stack<ISelection>();

        // 행동 선택창 열기
        OpenSelection(actionSelection);
    }

    public void OpenSelection(ISelection selection)
    {
        // 이전 열린 창 가져오기
        ISelection prevSelection = selectionLog.Count > 0 ? selectionLog.Peek() : null;

        // 이전 창은 닫고 현재 창 열기
        prevSelection?.CloseSelection();
        selection.OpenSelection();

        // 현재 창 로그에 추가
        selectionLog.Push(selection);
    }

    public void UndoSelection()
    {
        // 되돌릴 로그가 있는 경우
        if (selectionLog.Count > 1)
        {
            ISelection curSelection = selectionLog.Pop();
            ISelection prevSelection = selectionLog.Peek();

            curSelection.UndoSelection();
            prevSelection.ReopenSelection();
        }
    }

    public void SelectAction(BattleAction action)
    {
        this.action = action;
    }

    /***************************************************************
    * [ 타겟 선택 ]
    * 
    * 현재 행동의 타겟이 될 대상 선택
    ***************************************************************/

    public void SelectTarget(Entity target)
    {
        switch (action.actionType)
        {
            case ActionType.Attack:
                AttackAction attackAction = (AttackAction)action;
                attackAction.target = target;
                break;
        }
    }

    public TargetType GetTargetType()
    {
        switch (action.actionType)
        {
            case ActionType.Attack:
                return GetAttackTargetType(action);

            default:
                return TargetType.Caster;
        }
    }

    private TargetType GetAttackTargetType(BattleAction action)
    {
        Entity attacker = action.actor;

        return (attacker.AttackType == AttackType.Melee) ? TargetType.FrontEnemy : TargetType.Enemy;
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

        // actor.OnSelectAction(action, minIndex);
    }
}