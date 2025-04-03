using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public enum TargetType
{
    FrontEnemy,     // 적 진형 선열 1명
    Enemy,          // 적 진형 1명
    EnemyParty,     // 모든 적
    FrontMember,    // 파티 진형 선열 1명
    Member,         // 파티 맴버 1명
    PlayerParty,    // 모든 파티 맴버
    One,            // 모든 엔티티 중 하나
    Every,          // 모든 엔티티
    Self,            // 자기 자신
    None,           // 타겟 선택 패스
}

public class TargetSelection : MonoBehaviour, ISelection
{
    [Header("참조 스크립트")]
    [SerializeField] private TargetSelectionUI ui;
    [SerializeField] private ActionManager actionManager;

    [Header("선택창")]
    [SerializeField] private TurnSelection turnSelection;

    private Dictionary<TargetType, Action> targetSelectActions;
    private List<TargetSelectButton> selectButtons = new List<TargetSelectButton>();

    // 현재 변수
    BattleData battleData;
    private TargetType target;

    private void Awake()
    {
        battleData = BattleData.Instance;
        targetSelectActions = new Dictionary<TargetType, Action>
        {
            { TargetType.FrontEnemy, ActiveEnemyFront },
            { TargetType.Enemy, ActiveEnemy },
            { TargetType.EnemyParty, ActiveEnemyParty },
            { TargetType.Member, ActivePartyMember },
            { TargetType.PlayerParty, ActiveParty },
            { TargetType.Self, ActiveSelf },
            { TargetType.None, ActiveNone }
        };
    }

    public void InitSelectableEntities()
    {
        List<GameObject> entityList = battleData.EnemyList
            .Select(entity => entity.gameObject)
            .Concat(battleData.CharacterList.Select(entity => entity.gameObject))
            .ToList();

        // 각 엔티티 오브젝트마다 타겟 선택 버튼 생성
        AddButtonToList(entityList);
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
        BattleAction action = battleData.SelectionData.action;
        target = action.GetTargetType();

        // 타겟 활성화
        ActiveTarget(target);
    }

    public void CloseSelection()
    {
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
    * [ 타겟 선택 버튼 활성화 ]
    * 
    * 선택할 타겟 범위에 따른 타겟 표시 처리
    ***************************************************************/

    private bool IsAlive(Entity target) => !target.IsDead;
    private bool IsEnemy(Entity target) => target is Monster;
    private bool IsMember(Entity target) => target is Character;
    private bool IsFront(Entity target) => target.Position == BattlePosition.Front;
    private bool IsTargetSelf(Entity target) => target.Equals(battleData.SelectionData.actor);

    private void ActiveTarget(TargetType targetType)
    {
        if (targetSelectActions.TryGetValue(targetType, out var action))
        {
            action.Invoke();
        }
    }

    private void ActiveEnemyFront()
    {
        if (!BattleData.Instance.IsLivingEnemyFront)
        {
            // 전위가 없다면 모든 적 선택 가능
            ActiveEnemy();

            return;
        }

        // 적 그룹으로 카메라 돌리기
        BattleCameraDirector.Instance.FocusEnemyGroup();

        // 전위에 있는 적만 선택
        ActiveButtons((target) => IsEnemy(target) && IsFront(target));
    }

    private void ActiveEnemy()
    {
        // 적 그룹으로 카메라 돌리기
        BattleCameraDirector.Instance.FocusEnemyGroup();

        // 적만 선택
        ActiveButtons((target) => IsEnemy(target));
    }

    private void ActiveEnemyParty()
    {
        // 적 그룹으로 카메라 돌리기
        BattleCameraDirector.Instance.FocusEnemyGroup();

        // 모든 적 선택
        MultiSelectButtons((target) => IsEnemy(target));
    }

    private void ActivePartyMember()
    {
        // 아군 그룹으로 카메라 돌리기
        BattleCameraDirector.Instance.FocusPlayerGroup();

        // 아군만 선택
        ActiveButtons((target) => !IsEnemy(target));
    }

    private void ActiveParty()
    {
        // 아군 그룹으로 카메라 돌리기
        BattleCameraDirector.Instance.FocusPlayerGroup();

        // 모든 아군 선택
        MultiSelectButtons(target => !IsEnemy(target));
    }

    private void ActiveSelf()
    {
        // 아군 그룹으로 카메라 돌리기
        BattleCameraDirector.Instance.FocusPlayerGroup();

        // 자기 자신만 선택
        ActiveButtons((target) => IsTargetSelf(target));
    }

    private void ActiveNone()
    {
        // 타겟을 선택하지 않는 경우 스킵
        actionManager.SelectTargets(null);
    }

    private void ActiveButtons(Func<Entity, bool> activeCondition)
    {
        TargetSelectButton firstSelectButton = null;

        // 특정 버튼만 활성화
        foreach (TargetSelectButton button in selectButtons)
        {
            Entity target = button.targetEntity;

            // 살아있는 엔티티 중 조건에 맞는 엔티티의 버튼만 활성화
            button.interactable = IsAlive(target) && activeCondition(target);

            // 활성화된 버튼이 없을 경우
            if (firstSelectButton == null && button.interactable)
            {
                // 임시로 첫번째 버튼 저장
                firstSelectButton = button;
            }
        }

        // 이전 버튼 선택
        if (TargetSelectButton.lastSelected == null || TargetSelectButton.lastSelected.interactable == false)
        {
            // 이전에 선택한 버튼을 선택할 수 없는 경우 선택가능한 첫 버튼 선택
            TargetSelectButton.lastSelected = firstSelectButton;
        }

        EventSystem.current.SetSelectedGameObject(TargetSelectButton.lastSelected.gameObject);
    }

    private void MultiSelectButtons(Func<Entity, bool> selectCondition)
    {
        TargetSelectButton firstSelectButton = null;

        // 특정 버튼만 활성화
        foreach (TargetSelectButton button in selectButtons)
        {
            Entity target = button.targetEntity;

            // 살아있는 엔티티 중 조건에 맞는 엔티티의 버튼만 활성화 및 선택
            button.interactable = IsAlive(target) && selectCondition(target);

            if (button.interactable)
            {
                button.MultiSelected();

                if (firstSelectButton == null)
                {
                    firstSelectButton = button;
                }
            }
        }

        // 활성화 된 버튼 중 아무(첫번째) 버튼 선택
        EventSystem.current.SetSelectedGameObject(firstSelectButton.gameObject);
    }

    public void OnSelect()
    {
        List<Entity> list = new List<Entity>();

        foreach (TargetSelectButton button in selectButtons)
        {
            if (button.IsSelected) list.Add(button.targetEntity);
        }

        actionManager.SelectTargets(list);
    }

    private void DeactiveAllButtons()
    {
        // 모든 버튼 비활성화
        foreach (TargetSelectButton button in selectButtons)
        {
            button.interactable = false;

            // 멀티 선택된 버튼도 전부 초기화
            button.DeselectedMultiButton();
        }

        // 선택 버튼 초기화
        EventSystem.current.SetSelectedGameObject(null);
    }
}