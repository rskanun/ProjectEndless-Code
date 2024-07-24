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

public class TargetSelection : MonoBehaviour
{
    [Header("참조 스크립트")]
    [SerializeField] private TargetSelectionUI ui;
    [SerializeField] private ActionManager actionManager;

    private List<TargetSelectButton> selectionButtons = new List<TargetSelectButton>();

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
            TargetSelectButton selectButton = ui.CreateSelectButton(target, entityObj.transform.position);

            selectionButtons.Add(selectButton);
        }
    }

    public void OpenSelection(TargetType targetType)
    {
        // 타겟 활성화
        ActiveTarget(targetType);

        // 현재 타겟 범위 설정
        target = targetType;
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

    private void ActiveButtons(System.Action<TargetSelectButton> activeAction)
    {
        foreach (TargetSelectButton button in selectionButtons)
        {
            activeAction(button);

            if (EventSystem.current.currentSelectedGameObject == false)
            {
                HoverFirst(button);
            }
        }
    }

    private void HoverFirst(TargetSelectButton hoverButton)
    {
        hoverButton.OnHover();
    }

    public void OnSelect(Entity target)
    {
        actionManager.SelectTarget(target);
    }

    private void DeactiveAllButtons()
    {
        foreach (TargetSelectButton button in selectionButtons)
        {
            button.SetActive(false);
        }
    }
}