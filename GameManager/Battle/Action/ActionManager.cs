using System.Collections.Generic;
using UnityEngine;

public class SelectionData
{
    public Character actor;
    public BattleAction action;
}

public class ActionManager : MonoBehaviour
{
    [Header("컨트롤러")]
    [SerializeField] private BattleController controller;

    [Header("선택창")]
    [SerializeField] private ActionSelection actionSelection;
    [SerializeField] private SkillSelection skillSelection;
    [SerializeField] private ItemSelection itemSelection;
    [SerializeField] private TargetSelection targetSelection;
    [SerializeField] private TurnSelection turnSelection;

    [Header("참조 스크립트")]
    [SerializeField] private SurveyManager checkingManager;

    // 현재 열린 창
    private Stack<ISelection> selectionLog;

    // 현재 턴 정보
    private SelectionData selectionData;

    private void Awake()
    {
        selectionData = CurrentBattleData.Instance.SelectionData;
    }

    public void OpenSelection(ISelection selection)
    {
        // 이전 선택창이 있으면 닫기
        if (selectionLog.Count > 0)
        {
            ISelection prevSelection = selectionLog.Peek();
            prevSelection.CloseSelection();
        }

        // 다음 열 선택창 로그에 추가
        selectionLog.Push(selection);

        // 다음 선택창 열기
        selection.OpenSelection();
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

    public void SetSubController(IControlState subController)
    {
        controller.SetSubController(subController);
    }

    public void OnSelect(Character actor)
    {
        selectionData.actor = actor;

        // 로그 초기화
        selectionLog = new Stack<ISelection>();

        // 행동 선택창 열기
        OpenSelection(actionSelection);
    }

    /***************************************************************
    * [ 행동 선택 ]
    * 
    * 다음에 취할 행동 선택
    ***************************************************************/

    public void SelectAction(BattleAction action)
    {
        selectionData.action = action;

        // 다음 선택창 열기
        ISelection nextSelection = GetNextSelection(action.ActionType);
        OpenSelection(nextSelection);
    }

    public void SelectSkill(Skill skill)
    {
        // 선택한 스킬 등록
        SkillAction action = (SkillAction)selectionData.action;

        action.castSkill = skill;
        action.remainTurn = action.actor.GetLastTurn(skill.CostTurn);

        // 다음 선택창 열기
        OpenSelection(targetSelection);
    }

    public void SelectItem(Consumable item)
    {
        // 선택한 아이템 등록
        ItemAction action = (ItemAction)selectionData.action;

        action.usingItem = item;

        // 다음 선택창 열기
        OpenSelection(targetSelection);
    }

    private ISelection GetNextSelection(ActionType type)
    {
        ISelection nextSelection = type switch
        {
            // 도망과 대기는 타겟 선택 없이 바로 턴 선택
            ActionType.Run or ActionType.Wait => turnSelection,

            // 스킬일 경우 스킬 선택
            ActionType.Skill => skillSelection,

            // 아이템일 경우 아이템 선택
            ActionType.Item => itemSelection,

            // 나머지는 전부 타겟 선택
            _ => targetSelection
        };

        return nextSelection;
    }

    /***************************************************************
    * [ 타겟 선택 ]
    * 
    * 현재 행동의 타겟이 될 대상 선택
    ***************************************************************/

    public void SelectTargets(List<Entity> targets)
    {
        selectionData.action.SetTarget(targets);

        // 턴 선택창 열기
        OpenSelection(turnSelection);
    }

    /***************************************************************
    * [ 턴 선택 ]
    * 
    * 현재 행동이 배치될 턴 설정
    ***************************************************************/

    public void SelectTurn(float turn, int index)
    {
        Character actor = selectionData.actor;
        BattleAction action = selectionData.action;

        // 턴 선택창 닫기
        turnSelection.CloseSelection();

        // 턴 적용
        action.remainTurn = turn;

        // 선택한 데이터를 종합한 행동 실행
        actor.OnSelectAction(action, index);
    }

    /***************************************************************
    * [ 전황 살피기 ]
    * 
    * 현재 타임라인에 등록된 행동들이 어떤 행동들인지 확인
    ***************************************************************/

    public void SurveyingBattle()
    {
        // 행동 선택창 닫기 숨기기
        actionSelection.CloseSelection();

        // 전황 살피기
        checkingManager.OnStartSurvey();
    }

    public void ReturnToActionSelect()
    {
        // 다시 행동 선택창으로 돌아오기
        checkingManager.OnEndSurvey();
        actionSelection.ReopenSelection();
    }
}