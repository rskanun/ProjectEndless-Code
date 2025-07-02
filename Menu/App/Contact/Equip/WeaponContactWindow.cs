using UnityEngine;

public class WeaponContactWindow : ContactWindow
{
    [Header("연락처 오브젝트")]
    [SerializeField] private GameObject contactPrefab;
    [SerializeField] private Transform contactTrans;

    protected override void InitContact()
    {
        // 플레이어가 가진 모든 무기 목록 띄우기
        foreach (var weapon in InventoryData.Instance.GetItems(ItemType.Weapon))
        {

        }
    }
}