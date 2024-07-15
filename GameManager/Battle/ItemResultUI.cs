using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemResultUI : MonoBehaviour
{
    public Image itemIcon;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemCount;

    public void SetItemInfo(Item dropitem, int count)
    {
        itemName.text = dropitem.Name;
        itemCount.text = $"x {count:D2}";
    }
}