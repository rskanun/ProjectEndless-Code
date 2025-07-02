using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipContact : Contact
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI nameField;
    [SerializeField] private GameObject equipMark;

    public void UpdateInfo(Equip equip, bool isEquipped)
    {
        icon.sprite = equip.IconSprite;
        nameField.text = equip.Name;
        equipMark.SetActive(isEquipped);
    }
}