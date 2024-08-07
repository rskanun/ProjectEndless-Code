using System;
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

    private Dictionary<TargetType, Action> targetSelectActions;
    private List<TargetSelectButton> selectButtons = new List<TargetSelectButton>();

    // 현재 선택 가능한 타겟 범위
    private TargetType target;

    private void Awake()
    {
        targetSelectActions = new Dictionary<TargetType, Action>
        {
            { TargetType.FrontEnemy, ActiveEnemyFront },
            { TargetType.Enemy, ActiveEnemy },
            { TargetType.EnemyParty, ActiveEnemyParty },
            { TargetType.PartyMember, ActivePartyMember },
            { TargetType.PlayerParty, ActiveParty },
            { TargetType.Caster, ActiveCaster }
        };
    }

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
            TargetSelectButton selectButton = ui.CreateSelectButton(target, entityObj.transform.position);

            selectButtons.Add(selectButton);
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

        // 컨트롤러 활성화
        controller.ActiveController();
    }

    public void UndoSelection()
    {
        CloseSelection();
    }

    /***************************************************************
    * [ 타겟 선택 버튼 활성화 ]
    * 
    * 선택할 타겟 범위에 따른 타겟 표시 처리
    ***************************************************************/

    private bool IsAlive(Entity target) => !target.IsDead;
    private bool IsEnemy(Entity target) => target is Monster;
    private bool IsFront(Entity target) => target.Position == BattlePosition.Front;

    private void ActiveTarget(TargetType targetType)
    {
        if (targetSelectActions.TryGetValue(targetType, out var action))
        {
            action.Invoke();
        }
    }

    private void ActiveEnemyFront()
    {
        if (BattleData.Instance.EnemyFrontCount <= 0)
        {
            // 전위가 없다면 모든 적 선택 가능
            ActiveEnemy();

            return;
        }

        // 전위에 있는 적만 선택
        ActiveButtons((target) => IsEnemy(target) && IsFront(target));
    }

    private void ActiveEnemy()
    {
        // 적만 선택
        ActiveButtons((target) => IsEnemy(target));
    }

    private void ActiveEnemyParty()
    {

    }

    private void ActivePartyMember()
    {
        // 아군만 선택
        ActiveButtons((target) => !IsEnemy(target));
    }

    private void ActiveParty()
    {

    }

    private void ActiveCaster()
    {

    }

    private void ActiveButtons(Func<Entity, bool> selectCondition)
    {
        // 특정 버튼만 활성화
        foreach (TargetSelectButton button in selectButtons)
        {
            Entity target = button.targetEntity;

            // 살아있는 엔티티 중 조건에 맞는 엔티티의 버튼만 활성화
            button.interactable = IsAlive(target) && selectCondition(target);
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
        foreach (TargetSelectButton button in selectButtons)
        {
            button.interactable = false;
        }
    }

    /***************************************************************
    * [ 타겟 선택 ]
    * 
    * 선택할 타겟 지정 및 타겟 선택 처리
    ***************************************************************/

    public TargetSelectButton GetNextButton()
    {
        TargetSelectButton curButton = GetCurrentTarget();

        return curButton.NextButton;
    }

    public TargetSelectButton GetPrevButton()
    {
        TargetSelectButton curButton = GetCurrentTarget();

        return curButton.PrevButton;
    }

    private TargetSelectButton GetCurrentTarget()
    {
        if (selectButtons.Count <= 0)
        {
            return null;
        }

        if (TargetSelectButton.selectedButton == null)
        {
            TargetSelectButton.selectedButton = selectButtons[0];
        }

        return TargetSelectButton.selectedButton;
    }
}