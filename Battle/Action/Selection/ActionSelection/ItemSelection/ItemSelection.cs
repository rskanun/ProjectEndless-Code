using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSelection : MonoBehaviour, ISelection
{
    [Header("참조 스크립트")]
    [SerializeField] private ActionManager actionManager;
    [SerializeField] private ItemSelectionUI ui;

    // 현재 아이템창 내 아이템 정보 오브젝트
    private List<GameObject> itemInfoList = new List<GameObject>();

    // 마지막 선택 버튼
    private GameObject lastSelected;

    public void OpenSelection()
    {
        // 아이템창 열기
        ui.SetActiveWindow(true);

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
        // 현재 턴인 캐릭터
        Character actor = BattleData.Instance.SelectionData.actor;

        // 모션 없이 선택창에 맞게 카메라 이동
        BattleCameraDirector.Instance.FocusSelection(actor.gameObject);

        // 기존 아이템창 열기와 동일한 흐름
        OpenSelection();
    }

    public void UndoSelection()
    {
        // 아이템창 닫기
        CloseSelection();
    }

    /***************************************************************
    * [ 아이템 목록 관리 ]
    * 
    * 소비 아이템 선택창의 구성 UI 관리
    ***************************************************************/

    public void UpdateItemInfo()
    {
        Dictionary<Item, int> items = GetConsumableItems();

        ClearItemInfos();       // 아이템 목록 초기화
        InitItemInfo(items);    // 아이템 목록 생성
    }

    private void ClearItemInfos()
    {
        // 모든 아이템 목록 초기화
        for (int i = itemInfoList.Count - 1; i >= 0; i--)
        {
            RemoveItemInfoObject(itemInfoList[i]);
        }
    }

    private void RemoveItemInfoObject(GameObject itemInfoObj)
    {
        // 만약 마지막으로 선택한 아이템일 경우
        if (lastSelected == itemInfoObj)
        {
            // 마지막 선택 아이템에서 삭제
            lastSelected = null;
        }

        // 아이템 삭제
        itemInfoList.Remove(itemInfoObj);
        Destroy(itemInfoObj);
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
            OnItemClicked(consumable, itemInfoObj);
        });

        return itemInfoObj;
    }

    private void OnItemClicked(Consumable consumable, GameObject itemInfoObj)
    {
        actionManager.SelectItem(consumable);
        lastSelected = itemInfoObj;
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