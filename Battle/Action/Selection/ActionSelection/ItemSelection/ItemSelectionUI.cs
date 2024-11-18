using TMPro;
using UnityEngine;

public class ItemSelectionUI : MonoBehaviour
{
    [Header("아이템창 구성요소")]
    public GameObject selectionWindow;
    public GameObject itemInfoPrefab;
    public Transform container;
    public TextMeshProUGUI descriptionText;

    public void SetActiveWindow(bool isActive)
    {
        selectionWindow.SetActive(isActive);
    }

    public void SetDescription(string description)
    {
        descriptionText.text = description;
    }

    public GameObject CreateItemInfo(Consumable item)
    {
        GameObject itemInfoObj = Instantiate(itemInfoPrefab, container);

        // 생성된 오브젝트 리턴
        return itemInfoObj;
    }
}