using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ActionManager : MonoBehaviour
{
    [Header("선택창")]
    [SerializeField] private ActionSelection actionSelection;
    [SerializeField] private TargetSelection targetSelection;
    [SerializeField] private TurnSelection turnSelection;

    // 참조 데이터
    private BattleData battleData;

    // 현재 열린 창
    private Stack<ISelection> selectionLog;
    public ISelection openSelection
    {
        get { return selectionLog.Count > 0 ? selectionLog.Peek() : null; }
    }

    // 현재 턴 정보
    private BattleAction action;
    private Character actor;

    private void Awake()
    {
        battleData = BattleData.Instance;
    }

    public void OpenSelection(Character actor)
    {
        this.actor = actor;

        // 로그 초기화
        selectionLog = new Stack<ISelection>();

        // 행동 선택창 열기
        OpenActionSelection();
    }

    public void OpenActionSelection()
    {
        // 행동 선택 창 열기
        actionSelection.OpenSelection(actor);

        // 현재 창 로그에 추가
        selectionLog.Push(actionSelection);
    }

    public void UndoSelection()
    {
        // 되돌릴 로그가 있는 경우
        if (selectionLog != null && selectionLog.Count > 1)
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

        // 행동 선택창 닫기
        actionSelection.CloseSelection();

        // 대상 선택창 열기
        targetSelection.OpenSelection(action.GetTargetType());

        // 로그 추가
        selectionLog.Push(targetSelection);
    }

    /***************************************************************
    * [ 타겟 선택 ]
    * 
    * 현재 행동의 타겟이 될 대상 선택
    ***************************************************************/

    public void SelectTarget(Entity target)
    {
        List<Entity> targets = new List<Entity>() { target };

        SelectTargets(targets);
    }

    public void SelectTargets(List<Entity> targets)
    {
        action.SetTarget(targets);

        // 대상 선택창 닫기
        targetSelection.CloseSelection();

        // 턴 선택창 열기
        turnSelection.OpenSelection(action);

        // 로그 추가
        selectionLog.Push(turnSelection);
    }

    /***************************************************************
    * [ 턴 선택 ]
    * 
    * 현재 행동이 배치될 턴 설정
    ***************************************************************/

    public void SelectTurn(float turn, int index)
    {
        action.remainTurn = turn;

        actor.OnSelectAction(action, index);
    }
}