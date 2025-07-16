using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class WeaponContactWindow : ContactWindow
{
    [SerializeField] private ContactApp app;

    [Header("연락처 오브젝트")]
    [SerializeField] private GameObject contactPrefab;
    [SerializeField] private Transform contactTrans;

    private List<GameObject> contactList = new();
    private GameObject firstSelect;

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

            // 해당 캐릭터가 들고 있는 무기를 먼저, 없다면 가장 첫 무기를 먼저 선택
            if (app.SelectCharacter.MainWeapon == weapon) firstSelect = contactObj;
            if (firstSelect == null) firstSelect = contactObj;
        }
    }

    protected override IEnumerator OpenAnimation()
    {
        ControlContext.Instance.KeyLock();

        yield return StartCoroutine(base.OpenAnimation());

        // 목록을 다 불러온 후 처음으로 선택할 무기 설정
        if (firstSelect != null)
            EventSystem.current.SetSelectedGameObject(firstSelect);

        ControlContext.Instance.KeyUnlock();
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