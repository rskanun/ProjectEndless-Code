using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponContactWindow : ContactWindow
{
    [SerializeField] private ContactApp app;

    [Header("연락처 오브젝트")]
    [SerializeField] private GameObject contactPrefab;
    [SerializeField] private Transform contactTrans;

    private List<GameObject> contactList = new();

    private void OnDisable()
    {
        // 해당 창이 비활성화 될 때, 모든 오브젝트 목록을 지우기
        foreach (GameObject obj in contactList)
        {
            Destroy(obj);
        }
    }

    protected override void InitContact()
    {
        // 플레이어가 가진 모든 무기 목록 띄우기
        foreach ((Item item, int count) in InventoryData.Instance.GetItems(ItemType.Weapon))
        {
            Weapon weapon = item as Weapon;

            // 해당 무기 정보를 토대로 한 연락처(=정보) 오브젝트 생성
            GameObject contactObj = Instantiate(contactPrefab, contactTrans);
            EquipContact contact = contactObj.GetComponent<EquipContact>();

            // 정보 및 핸들러 등록
            contact.UpdateInfo(weapon, count, IsEquipAnyone(weapon));

            // 후에 파괴를 위한 리스트에 추가
            contactList.Add(contactObj);
        }
    }

    /// <summary>
    /// 누군가 해당 장비를 착용하고 있는 지 여부를 리턴
    /// </summary>
    /// <param name="weapon">확인하고자 하는 장비</param>
    /// <returns></returns>
    private bool IsEquipAnyone(Weapon weapon)
    {
        // 언락된 캐릭터만 뽑아내기
        List<CharacterData> unlockChrs = PartyData.Instance.Characters.Where(chr => chr.IsUnlocked).ToList();

        foreach (CharacterData chr in unlockChrs)
        {
            // 언락된 캐릭터들에 한에서만 착용 중인 지 체크
            if (chr.MainWeapon == weapon || chr.OffWeapon == weapon) return true;
        }

        return false;
    }
}