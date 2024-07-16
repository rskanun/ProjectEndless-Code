using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{
    [Header("참조 스크립트")]
    [SerializeField] private SelectionUI ui;
    [SerializeField] private ActionManager actionManager;

    private Dictionary<Button, GameObject> btnToEntityMap = new Dictionary<Button, GameObject>();
    private List<Button> allEntities = new List<Button>();
    private List<Button> enemyPartyFront = new List<Button>();
    private List<Button> enemyParty = new List<Button>();
    private List<Button> playerParty = new List<Button>();

    public void InitSelectableEntities()
    {
        InitSelectableEnemies();
        InitSelectableMembers();
    }

    private void InitSelectableEnemies()
    {
        List<GameObject> enemyList = BattleData.Instance.EnemyList;
        List<GameObject> enemyFrontList = BattleData.Instance.EnemyFrontList;

        foreach (GameObject enemyObj in enemyList)
        {
            AddButtonToList(enemyObj, enemyParty);
            if (enemyFrontList.Contains(enemyObj))
            {
                enemyPartyFront.Add(enemyParty[enemyParty.Count - 1]);
            }
        }
    }

    private void InitSelectableMembers()
    {
        List<GameObject> partyList = BattleData.Instance.PartyList;

        foreach (GameObject memberObj in partyList)
        {
            AddButtonToList(memberObj, playerParty);
        }
    }

    private void AddButtonToList(GameObject obj, List<Button> list)
    {
        Button selectButton = ui.CreateSelectButton(obj.transform.position);

        list.Add(selectButton);
        allEntities.Add(selectButton);

        btnToEntityMap[selectButton] = obj;
    }

    public void SelectFront()
    {
        if (BattleData.Instance.EnemyFrontCount <= 0)
        {
            SelectEntities(enemyParty);
        }
        else
        {
            SelectEntities(enemyPartyFront);
        }
    }

    public void SelectEnemy()
    {
        SelectEntities(enemyParty);
    }

    public void SelectParty()
    {
        SelectEntities(playerParty);
    }

    private void SelectEntities(List<Button> selectedEntities)
    {
        foreach (Button button in allEntities)
        {
            button.interactable = selectedEntities.Contains(button);
        }

        // 첫 버튼의 경우 활성화 상태로 전환
        selectedEntities[0].Select();
    }

    public void OnSelect(Button selectButton)
    {
        if (btnToEntityMap.TryGetValue(selectButton, out GameObject target))
        {
            actionManager.SelectTarget(target);
        }
    }
}