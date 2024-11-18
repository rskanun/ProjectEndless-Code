using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BattleResultUI : MonoBehaviour
{
    [Header("아이템 정보 프리팹")]
    public GameObject itemPrefab;
    [Header("결과창 구성 오브젝트")]
    public GameObject resultWindow;
    public TextMeshProUGUI goldAmount;
    public Transform itemContainer;

    public void OpenResult()
    {
        // UI 열기
        resultWindow.SetActive(true);

        // UI 정보 설정
        CurrentBattleData battleData = CurrentBattleData.Instance;

        SetGold(battleData.TotalAmount);
        SetDropItems(battleData.DropItems);
    }

    public void OnConfirm()
    {
        // UI 닫고 기존 씬으로 이동
        resultWindow.SetActive(false);
    }

    private void SetGold(int gold)
    {
        goldAmount.text = gold + " G";
    }

    private void SetDropItems(Dictionary<Item, int> dropItems)
    {
        foreach (Item item in dropItems.Keys)
        {
            GameObject itemObj = Instantiate(itemPrefab, itemContainer);
            ItemResultUI itemUI = itemObj.GetComponent<ItemResultUI>();

            itemUI.SetItemInfo(item, dropItems[item]);
        }
    }
}