using UnityEngine;
using UnityEngine.EventSystems;

public class ActionSelection : MonoBehaviour, ISelection
{
    [Header("참조 스크립트")]
    [SerializeField] private ActionManager actionManager;
    [SerializeField] private ActionSelectionUI actionSelectionUI;
    [SerializeField] private ActionSelectionController controller;

    [Header("서브 선택창")]
    [SerializeField] private SkillSelection skillSelection;
    [SerializeField] private ItemSelectionUI itemSelectionUI;

    // 현재 행동을 진행 중인 캐릭터
    private Character actor;

    public void OpenSelection(Character actor)
    {
        this.actor = actor;

        // 선택창 열기
        actionSelectionUI.OpenSelectionWindow();

        // 컨트롤러 설정
        actionManager.SetSubController(controller);
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
        // 현재 순서인 캐릭터 그대로 행동 선택창 열기
        OpenSelection(actor);
    }

    public void UndoSelection()
    {
        // 행동 선택창이 마지막이므로 되돌리기 X
    }

    public void OpenSkillSelection()
    {
        // 행동 선택창 닫기
        actionSelectionUI.CloseSelectionWindow();

        // 스킬 선택창 열기
        skillSelection.OpenSelection(actor.SkillList);

        // 로그 추가
        actionManager.AddLog(skillSelection);
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
        // 스킬 행동 생성
        SkillAction action = new SkillAction();

        action.castSkill = skill;
        action.actor = actor;

        // 선택한 행동 알리기
        actionManager.SelectAction(action);
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