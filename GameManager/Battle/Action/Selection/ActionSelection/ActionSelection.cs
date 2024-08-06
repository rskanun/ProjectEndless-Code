using UnityEngine;

public class ActionSelection : MonoBehaviour, ISelection
{
    [Header("참조 스크립트")]
    [SerializeField] private ActionManager actionManager;
    [SerializeField] private ActionSelectionController controller;
    [SerializeField] private ActionSelectionUI actionSelectionUI;

    [Header("선택창")]
    [SerializeField] private SkillSelection skillSelection;
    [SerializeField] private ItemSelection itemSelection;
    [SerializeField] private TargetSelection targetSelection;

    public void OpenSelection()
    {
        // 컨트롤러 활성화
        controller.ActiveController();

        // 선택창 열기
        actionSelectionUI.OpenSelectionWindow();
    }

    public void CloseSelection()
    {
        // 컨트롤러 비활성화
        controller.DeactiveController();

        // 선택창 닫기
        actionSelectionUI.CloseSelectionWindow();
    }

    public void ReopenSelection()
    {
        // 행동 선택창 열기
        OpenSelection();
    }

    public void UndoSelection()
    {
        // 해당 선택창에선 뒤로가기가 없음
    }

    /***************************************************************
    * [ 행동 선택 ]
    * 
    * 현재 턴인 캐릭터가 어떤 행동을 취할 지 선택창 생성 및 처리
    ***************************************************************/

    public void OnSelectAttack()
    {
        // 공격 행동 생성
        Character actor = actionManager.GetActor();
        AttackAction action = actor.CreateAttackAction();

        // 다음 선택창으로 넘기기
        OnSelectAction(action);
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

    private void OnSelectAction(BattleAction action)
    {
        // 선택한 행동 저장
        actionManager.SelectAction(action);

        // 다음 선택창 열기
        actionManager.OpenSelection(targetSelection);
    }
}