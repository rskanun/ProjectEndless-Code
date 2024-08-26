using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSelection : MonoBehaviour, ISelection
{
    [Header("참조 스크립트")]
    [SerializeField] private ActionSelection actionSelection;
    [SerializeField] private ItemSelectionUI ui;

    // 현재 아이템창 내 아이템 정보 오브젝트
    private List<GameObject> itemInfoList = new List<GameObject>();

    // 마지막 선택 버튼
    private GameObject lastSelected;

    public void OpenSelection()
    {
        // 아이템창 열기
        ui.SetActiveWindow(true);

        // 아이템 정보 업데이트
        UpdateItemInfo();

        // 초기 아이템 선택
        SelectLastButton();
    }

    public void CloseSelection()
    {
        // 아이템창 닫기
        ui.SetActiveWindow(false);
    }

    public void ReopenSelection()
    {
        // 이전 유지된 데이터를 기반으로 아이템창 열기
        ui.SetActiveWindow(true);

        // 초기 아이템 선택
        SelectLastButton();
    }

    public void UndoSelection()
    {
        // 아이템창 닫기
        CloseSelection();
    }

    /***************************************************************
    * [ UI 설정 ]
    * 
    * 스킬 선택창의 구성 UI 설정
    ***************************************************************/

    public void UpdateItemInfo()
    {
        // 처음 여는 경우 아이템 정보 배치
        if (itemInfoList.Count == 0)
        {
            Dictionary<Item, int> items = GetConsumableItems();

            InitItemInfo(items);
        }
        else
        {
            // 처음이 아닌 경우 아이템 카운트 업데이트
            UpdateItemCount(lastSelected);
        }
    }

    private void InitItemInfo(Dictionary<Item, int> items)
    {
        foreach (var itemPair in items)
        {
            Consumable consumable = itemPair.Key as Consumable;
            int itemCount = itemPair.Value;

            // 아이템 정보 오브젝트 생성
            GameObject itemInfoObj = CreateItemInfoObject(consumable, itemCount);

            // 아이템 선택을 위한 리스트에 추가
            itemInfoList.Add(itemInfoObj);
        }
    }

    private GameObject CreateItemInfoObject(Consumable consumable, int count)
    {
        // 아이템 정보를 담은 오브젝트 생성
        GameObject itemInfoObj = ui.CreateItemInfo(consumable);

        // 아이템 정보 설정
        ItemInfo itemInfo = itemInfoObj.GetComponent<ItemInfo>();
        itemInfo.SetItem(consumable, count);

        // hover 설정
        itemInfo.SetHoverHandler(() => ui.SetDescription(consumable.Description));

        // 버튼 클릭 설정
        itemInfo.SetClickHandler(() =>
        {
            OnItemClicked(itemInfo, consumable, itemInfoObj);
        });

        return itemInfoObj;
    }

    private void OnItemClicked(ItemInfo itemInfo, Consumable consumable, GameObject itemInfoObj)
    {
        actionSelection.OnSelectItem(consumable);
        lastSelected = itemInfoObj;
    }

    private void UpdateItemCount(GameObject itemInfoObj)
    {
        if (itemInfoObj == null) return;

        ItemInfo itemInfo = itemInfoObj.GetComponent<ItemInfo>();
        int count = itemInfo.GetCount();

        if (count <= 1) RemoveItemInfoObject(itemInfoObj);
        else itemInfo.SetCount(count - 1);
    }

    private void RemoveItemInfoObject(GameObject itemInfoObj)
    {
        // 만약 마지막으로 선택한 아이템일 경우
        if (lastSelected ==  itemInfoObj)
        {
            // 마지막 선택 아이템에서 삭제
            lastSelected = null;
        }

        // 아이템 삭제
        itemInfoList.Remove(itemInfoObj);
        Destroy(itemInfoObj);
    }

    private Dictionary<Item, int> GetConsumableItems()
    {
        return InventoryData.Instance.GetItems(ItemType.Consumable);
    }

    private void SelectLastButton()
    {
        if (itemInfoList.Count == 0) return;

        if (lastSelected == null)
        {
            // 이전에 선택한 버튼이 없는 경우 선택 가능한 첫번째 요소를 선택
            lastSelected = GetFirstUsableItem();
        }

        // 버튼 선택
        EventSystem.current.SetSelectedGameObject(lastSelected);

        // 아이템 설명 설정
        UpdateDescription(lastSelected);
    }

    private GameObject GetFirstUsableItem()
    {
        // 첫번째 선택 요소 찾기
        foreach (GameObject itemInfoObj in itemInfoList)
        {
            ItemInfo itemInfo = itemInfoObj.GetComponent<ItemInfo>();

            // 해당 아이템을 사용가능한 경우
            if (itemInfo.IsUsable())
            {
                // 해당 아이템을 첫번째 선택 요소로 반환
                return itemInfoObj;
            }
        }

        return null;
    }

    private void UpdateDescription(GameObject selectedItem)
    {
        ItemInfo item = selectedItem.GetComponent<ItemInfo>();
        ui.SetDescription(item.GetItem().Description);
    }
}