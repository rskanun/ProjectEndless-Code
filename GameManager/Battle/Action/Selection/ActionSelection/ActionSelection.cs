using System.Collections.Generic;
using UnityEngine;

public class ActionSelection : MonoBehaviour
{
    [Header("참조 스크립트")]
    [SerializeField] private ActionManager actionManager;
    [SerializeField] private ActionSelectionController controller;
    [SerializeField] private ActionSelectionUI actionSelectionUI;
    [SerializeField] private SkillSelectionUI skillSelectionUI;
    [SerializeField] private ItemSelectionUI itemSelectionUI;

    // 현재 열려있는 선택창
    private ISubActionSelection subSelection;

    // 현재 턴 정보
    private Character actor;

    public void OpenSelection(Character actor)
    {
        this.actor = actor;
        subSelection = null;

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
        skillSelectionUI.CloseSubSelection();
        itemSelectionUI.CloseSubSelection();
    }

    public void ReopenSelection()
    {
        if (subSelection != null)
        {
            // 이전에 서브 선택창을 열었다면 그 창 열기
            subSelection.ReopenSubSelection();
        }
        else
        {
            // 열었던 서브창이 없다면 행동 선택 메뉴 열기
            actionSelectionUI.OpenSelectionWindow();
        }
    }

    public void UndoSelection()
    {
        // 열려있는 서브 선택창이 있을 경우
        if (subSelection != null)
        {
            // 해당 창을 닫기
            subSelection.CloseSubSelection();
        }

        actionManager.UndoAction();
    }

    public void OpenSkillSelection()
    {
        // 현재 턴인 캐릭터가 사용가능한 스킬 목록 불러오기
        List<Skill> skillList = actor.SkillList;

        // 스킬 선택창 열기
        subSelection = skillSelectionUI;
        skillSelectionUI.OpenSkillSelection(skillList);
    }

    public void OpenItemSelection()
    {

    }
}