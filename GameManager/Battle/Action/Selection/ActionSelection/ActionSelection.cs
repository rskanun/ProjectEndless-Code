using UnityEngine;
using UnityEngine.EventSystems;

public class ActionSelection : MonoBehaviour, ISelection
{
    [Header("참조 스크립트")]
    [SerializeField] private ActionManager actionManager;
    [SerializeField] private ActionSelectionUI actionSelectionUI;
    [SerializeField] private ActionSelectionController controller;

    // 현재 턴인 캐릭터
    private Character actor;

    public void OpenSelection(SelectionData selectionData)
    {
        // 현재 턴인 캐릭터 설정
        actor = selectionData.actor;

        // 행동 선택창 열기
        OpenActionSelection(actor);
    }

    public void CloseSelection()
    {
        // 선택창 닫기
        actionSelectionUI.CloseSelectionWindow();

        // 컨트롤러 없애기
        actionManager.SetSubController(null);
    }

    public void ReopenSelection()
    {
        // 다시 선택창 열기
        OpenActionSelection(actor);
    }

    public void UndoSelection()
    {
        // 행동 선택창이 마지막이므로 되돌리기 X
    }

    private void OpenActionSelection(Character actor)
    {
        // 선택창 열기
        actionSelectionUI.OpenSelectionWindow();

        // 컨트롤러 설정
        actionManager.SetSubController(controller);
    }

    /***************************************************************
    * [ 행동 선택 ]
    * 
    * 현재 턴인 캐릭터가 어떤 행동을 취할 지 선택창 생성 및 처리
    ***************************************************************/

    public void OnSelectAttack()
    {
        // 공격 행동 생성
        AttackAction action = actor.CreateAttackAction();

        // 타겟 선택창으로 넘어가기
        actionManager.SelectAction(action);
    }

    public void OnSelectSkill()
    {
        // 스킬 행동 생성
        SkillAction action = new SkillAction();

        action.actor = actor;

        // 선택한 행동 알리기
        actionManager.SelectAction(action);
    }

    public void OnSelectItem()
    {
        // 아이템 행동 생성
        ItemAction action = new ItemAction();

        action.actor = actor;
        action.remainTurn = 0.0f;

        // 선택한 행동 알리기
        actionManager.SelectAction(action);
    }

    public void OnSelectWaiting()
    {
        // 대기 행동 생성
        WaitAction action = new WaitAction();

        action.actor = actor;
        action.remainTurn = 1.0f; // 최소 한 턴은 대기해야함

        // 선택한 행동 알리기
        actionManager.SelectAction(action);
    }

    public void OnSelectRun()
    {
        // 도주 행동 생성
        RunAction action = new RunAction();

        action.actor = actor;
        action.remainTurn = 1.0f;

        // 선택한 행동 알리기
        actionManager.SelectAction(action);
    }

    private void SelectAction(BattleAction action)
    {
        // 선택한 행동 보내기
        actionManager.SelectAction(action);

        // 선택 초기화
        EventSystem.current.SetSelectedGameObject(null);
    }
}