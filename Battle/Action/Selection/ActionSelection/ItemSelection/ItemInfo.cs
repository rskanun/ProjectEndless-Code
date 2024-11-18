using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemInfo : MonoBehaviour, ISelectHandler
{
    [Header("아이템 정보 구성요소")]
    public Image icon;
    public Button button;
    public GameObject unusablePanel;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemCount;

    // 아이템 정보
    private Consumable item;
    private int count;

    // 이벤트
    private Action hoverHandler;
    private Action clickHandler;

    public void SetItem(Consumable item, int count)
    {
        this.item = item;
        this.count = count;

        // 정보 적응
        icon.sprite = item.IconSprite;
        itemName.text = item.Name;
        itemCount.text = $"x{count}";
    }

    public Consumable GetItem()
    {
        return item;
    }

    public int GetCount()
    {
        return count;
    }

    public void SetCount(int count)
    {
        this.count = count;
        itemCount.text = $"x{count}";
    }

    public void OnHover()
    {
        if (IsUsable())
        {
            hoverHandler?.Invoke();

            // 해당 오브젝트 선택
            EventSystem.current.SetSelectedGameObject(gameObject);
        }
    }

    public void SetHoverHandler(Action handler)
    {
        hoverHandler = handler;
    }

    public void OnClick()
    {
        if (count > 0)
        {
            clickHandler?.Invoke();
        }
    }

    public void SetClickHandler(Action handler)
    {
        clickHandler = handler;
    }

    public void OnSelect(BaseEventData eventData)
    {
        hoverHandler?.Invoke();
    }

    public void SetUsable(bool isUsed)
    {
        button.interactable = isUsed;
        unusablePanel.SetActive(!isUsed);
    }

    public bool IsUsable()
    {
        return button.interactable;
    }
}