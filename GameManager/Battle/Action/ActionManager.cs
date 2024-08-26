using System.Collections.Generic;
using UnityEngine;

public class ActionManager : MonoBehaviour
{
    [Header("컨트롤러")]
    [SerializeField] private BattleController controller;

    [Header("선택창")]
    [SerializeField] private ActionSelection actionSelection;
    [SerializeField] private TargetSelection targetSelection;
    [SerializeField] private TurnSelection turnSelection;

    // 참조 데이터
    private BattleData battleData;

    // 현재 열린 창
    private Stack<ISelection> selectionLog;

    // 현재 턴 정보
    private BattleAction action;
    private Character actor;

    private void Awake()
    {
        battleData = BattleData.Instance;
    }

    public void SetSubController(IControlState subController)
    {
        controller.SetSubController(subController);
    }

    public void AddLog(ISelection openSelection)
    {
        selectionLog.Push(openSelection);
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
        AddLog(actionSelection);
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

        // 이전 선택창 닫기
        ISelection prevSelection = selectionLog.Peek();
        prevSelection.CloseSelection();

        // 대상 선택창 열기
        targetSelection.OpenSelection(action.GetTargetType());

        // 로그 추가
        AddLog(targetSelection);
    }

    /***************************************************************
    * [ 타겟 선택 ]
    * 
    * 현재 행동의 타겟이 될 대상 선택
    ***************************************************************/

    public void SelectTargets(List<Entity> targets)
    {
        action.SetTarget(targets);

        // 대상 선택창 닫기
        targetSelection.CloseSelection();

        // 턴 선택창 열기
        turnSelection.OpenSelection(action);

        // 로그 추가
        AddLog(turnSelection);
    }

    /***************************************************************
    * [ 턴 선택 ]
    * 
    * 현재 행동이 배치될 턴 설정
    ***************************************************************/

    public void SelectTurn(float turn, int index)
    {
        // 턴 선택창 닫기
        turnSelection.CloseSelection();

        // 턴 적용
        action.remainTurn = turn;

        // 선택한 데이터를 종합한 행동 실행
        actor.OnSelectAction(action, index);
    }
}