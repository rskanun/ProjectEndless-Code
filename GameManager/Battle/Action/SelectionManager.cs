using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectionManager : MonoBehaviour
{
    [Header("참조 스크립트")]
    [SerializeField] private SelectionUI ui;
    [SerializeField] private ActionManager actionManager;

    private List<SelectionButton> selectionButtons = new List<SelectionButton>();

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
            SelectionButton selectButton = ui.CreateSelectButton(target, entityObj.transform.position);

            selectionButtons.Add(selectButton);
        }
    }

    public void SelectFront()
    {
        if (BattleData.Instance.EnemyFrontCount <= 0)
        {
            SelectEnemy();
        }
        else
        {
            ActiveButtons((button) => button.EnemyFrontActive());
        }
    }

    public void SelectEnemy()
    {
        ActiveButtons((button) => button.EnemyActive());
    }

    public void SelectParty()
    {
        ActiveButtons((button) => button.PlayerPartyActive());
    }

    private void ActiveButtons(System.Action<SelectionButton> activeAction)
    {
        foreach (SelectionButton button in selectionButtons)
        {
            activeAction(button);

            if (EventSystem.current.currentSelectedGameObject == false)
            {
                HoverFirst(button);
            }
        }
    }

    private void HoverFirst(SelectionButton hoverButton)
    {
        hoverButton.OnHover();
    }

    public void OnSelect(Entity target)
    {
        actionManager.SelectTarget(target);

        // 모든 버튼 비활성화
        DeactiveAllButtons();
    }

    private void DeactiveAllButtons()
    {
        foreach (SelectionButton button in selectionButtons)
        {
            button.SetActive(false);
        }
    }
}