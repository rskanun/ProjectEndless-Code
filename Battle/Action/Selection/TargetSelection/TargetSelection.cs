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
    [SerializeField] private ActionManager actionManager;

    [Header("선택창")]
    [SerializeField] private TurnSelection turnSelection;

    private Dictionary<TargetType, Action> targetSelectActions;

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

        // 버튼 클릭 이벤트 등록
        InitClickHandler();
    }

    private void InitClickHandler()
    {
        // 모든 버튼에 클릭 이벤트 달아놓기
        TargetSelectButtonManager.Instance.RegisterClickHandler(()
            => OnSelectTargets());
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
        TargetSelectButtonManager.Instance.DeactiveAllButtons();
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
        ActiveButtons(target => IsEnemy(target) && IsFront(target));
    }

    private void ActiveEnemy()
    {
        // 적 그룹으로 카메라 돌리기
        BattleCameraDirector.Instance.FocusEnemyGroup();

        // 적만 선택
        ActiveButtons(target => IsEnemy(target));
    }

    private void ActiveEnemyParty()
    {
        // 적 그룹으로 카메라 돌리기
        BattleCameraDirector.Instance.FocusEnemyGroup();

        // 모든 적 선택
        MultiSelectButtons(target => IsEnemy(target));
    }

    private void ActivePartyMember()
    {
        // 아군 그룹으로 카메라 돌리기
        BattleCameraDirector.Instance.FocusPlayerGroup();

        // 아군만 선택
        ActiveButtons(target => !IsEnemy(target));
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
        ActiveButtons(target => IsTargetSelf(target));
    }

    private void ActiveNone()
    {
        // 타겟을 선택하지 않는 경우 스킵
        actionManager.SelectTargets(null);
    }

    private void ActiveButtons(Func<Entity, bool> activeCondition)
    {
        TargetSelectButtonManager.Instance.ActiveButtons(target
            => IsAlive(target) && activeCondition(target));
    }

    private void MultiSelectButtons(Func<Entity, bool> selectCondition)
    {
        TargetSelectButtonManager.Instance.SelectButtons(target
            => IsAlive(target) && selectCondition(target));
    }

    private void OnSelectTargets()
    {
        // 선택된 타겟들을 보내기
        actionManager.SelectTargets(TargetSelectButtonManager.Instance.GetSelectedTargets());
    }
}