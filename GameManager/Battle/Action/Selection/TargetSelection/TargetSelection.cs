using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum TargetType
{
    FrontEnemy,     // 적 진형 선열 1명
    Enemy,          // 적 진형 1명
    EnemyParty,     // 모든 적
    PartyMember,    // 파티 맴버 1명
    PlayerParty,    // 모든 파티 맴버
    Caster          // 사용자
}

public class TargetSelection : MonoBehaviour, ISelection
{
    [Header("참조 스크립트")]
    [SerializeField] private TargetSelectionUI ui;
    [SerializeField] private TargetSelectionController controller;
    [SerializeField] private ActionManager actionManager;

    [Header("선택창")]
    [SerializeField] private TurnSelection turnSelection;

    private List<OldTargetSelectButton> selectionButtons = new List<OldTargetSelectButton>();

    // 현재 선택 가능한 타겟 범위
    private TargetType target;

    public void InitSelectableEntities()
    {
        AddButtonToList(BattleData.Instance.EnemyList);
        AddButtonToList(BattleData.Instance.PartyList);
    }

    private void AddButtonToList(List<GameObject> entityList)
    {
        foreach (GameObject entityObj in entityList)
        {
            Entity target = entityObj.GetComponent<Entity>();
            OldTargetSelectButton selectButton = ui.CreateSelectButton(target, entityObj.transform.position);

            selectionButtons.Add(selectButton);
        }
    }

    public void OpenSelection()
    {
        // 현재 타겟 범위 설정
        target = actionManager.GetTargetType();

        // 타겟 활성화
        ActiveTarget(target);

        // 컨트롤러 활성화
        controller.ActiveController();
    }

    public void CloseSelection()
    {
        // 컨트롤러 비활성화
        controller.DeactiveController();

        // 모든 버튼 비활성화
        DeactiveAllButtons();
    }

    public void ReopenSelection()
    {
        // 이전 타겟 재활성화
        ActiveTarget(target);
    }

    public void UndoSelection()
    {
        CloseSelection();
    }

    /***************************************************************
    * [ 타겟 선택 ]
    * 
    * 선택할 타겟 범위에 따른 타겟 표시 처리
    ***************************************************************/

    private void ActiveTarget(TargetType targetType)
    {
        switch (targetType)
        {
            case TargetType.FrontEnemy:
                ActiveEnemyFront();
                break;

            case TargetType.Enemy:
                ActiveEnemy();
                break;

            case TargetType.EnemyParty:
                ActiveEnemyParty();
                break;

            case TargetType.PartyMember:
                ActivePartyMember();
                break;

            case TargetType.PlayerParty:
                ActiveParty();
                break;

            case TargetType.Caster:
                ActiveCaster();
                break;
        }
    }

    private void ActiveEnemyFront()
    {
        if (BattleData.Instance.EnemyFrontCount <= 0)
        {
            ActiveEnemy();
        }
        else
        {
            ActiveButtons((button) => button.EnemyFrontActive());
        }
    }

    private void ActiveEnemy()
    {
        ActiveButtons((button) => button.EnemyActive());
    }

    private void ActiveEnemyParty()
    {

    }

    private void ActivePartyMember()
    {
        ActiveButtons((button) => button.PlayerPartyActive());
    }

    private void ActiveParty()
    {

    }

    private void ActiveCaster()
    {

    }

    private void ActiveButtons(System.Action<OldTargetSelectButton> activeAction)
    {
        foreach (OldTargetSelectButton button in selectionButtons)
        {
            // 특정 버튼만 활성화
            activeAction(button);

            // 현재 선택된 버튼이 없을 경우
            if (EventSystem.current.currentSelectedGameObject == false)
            {
                // 처음 버튼을 선택
                button.OnHover();
            }
        }
    }

    public void OnSelect(Entity target)
    {
        // 타겟 선택
        actionManager.SelectTarget(target);

        // 턴 선택
        actionManager.OpenSelection(turnSelection);
    }

    private void DeactiveAllButtons()
    {
        // 모든 버튼 비활성화
        foreach (OldTargetSelectButton button in selectionButtons)
        {
            button.Deactive();
        }
    }
}