using UnityEngine;
using UnityEngine.EventSystems;

public class ActionSelection : MonoBehaviour, ISelection
{
    [Header("참조 스크립트")]
    [SerializeField] private ActionManager actionManager;
    [SerializeField] private ActionSelectionUI actionSelectionUI;
    [SerializeField] private ActionSelectionController controller;

    [Header("서브 선택창")]
    [SerializeField] private SkillSelectionUI skillSelectionUI;
    [SerializeField] private ItemSelectionUI itemSelectionUI;

    // 현재 열려있는 서브창
    private ISubActionSelection subSelection;

    // 현재 행동을 진행 중인 캐릭터
    private Character actor;

    public void OpenSelection(Character actor)
    {
        this.actor = actor;
        subSelection = null;

        // 선택창 열기
        actionSelectionUI.OpenSelectionWindow();

        // 컨트롤러 설정
        actionManager.SetSubController(controller);
    }

    public void CloseSelection()
    {
        if (subSelection != null)
        {
            // 서브창이 열려있으면 서브창 닫기
            subSelection.CloseSubSelection();

            // 컨트롤러 설정
            actionManager.SetSubController(controller);
        }
        else
        {
            // 선택창이 열려있으면 선택창 닫기
            actionSelectionUI.CloseSelectionWindow();

            // 컨트롤러 없애기
            actionManager.SetSubController(null);
        }
    }

    public void ReopenSelection()
    {
        if (subSelection != null)
        {
            // 서브창이 열린 적이 있다면, 서브창 열기
            subSelection.ReopenSubSelection();

            // 컨트롤러 없애기
            actionManager.SetSubController(null);
        }
        else
        {
            // 서브창이 열리지 않았다면 행동 선택창 열기
            actionSelectionUI.OpenSelectionWindow();

            // 컨트롤러 설정
            actionManager.SetSubController(controller);
        }
    }

    public void UndoSelection()
    {
        if (subSelection != null)
        {
            // 서브창이 열려있다면 닫기
            subSelection.CloseSubSelection();

            // 서브창 로그 초기화
            subSelection = null;
        }
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

        // 선택한 행동 알리기
        actionManager.SelectAction(action);
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

    private void SelectAction(BattleAction action)
    {
        // 선택한 행동 보내기
        actionManager.SelectAction(action);

        // 선택 초기화
        EventSystem.current.SetSelectedGameObject(null);
    }
}