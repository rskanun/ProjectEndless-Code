using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class AccessoryContactWindow : ContactWindow
{
    private enum AccessorySlot
    {
        Accessory1,
        Accessory2
    }

    [SerializeField] private NavigationGroup naviGroup;
    [SerializeField] private ContactApp app;
    [SerializeField] private AccessorySlot slot;

    [Header("연락처 오브젝트")]
    [SerializeField] private GameObject contactPrefab;
    [SerializeField] private Transform contactTrans;

    private List<GameObject> contactList = new();
    private Dictionary<GameObject, float> contactsY = new();
    private GameObject firstSelect;
    private EquipContact currentEquip;

    private float originContentSize;

    protected override void InitContact()
    {
        // 플레이어가 가진 모든 악세사리 목록 띄우기
        foreach ((Item item, int count) in InventoryData.Instance.GetItems(ItemType.Accessory))
        {
            Accessory accessory = item as Accessory;
            CharacterData character = app.SelectCharacter;

            // 해당 악세사리 정보를 토대로 한 연락처(=정보) 오브젝트 생성
            GameObject contactObj = Instantiate(contactPrefab, contactTrans);
            EquipContact contact = contactObj.GetComponent<EquipContact>();

            // 정보 및 핸들러 등록
            contact.UpdateInfo(accessory, count, IsEquipAnyone(accessory));
            contact.SetSelectAction(() => UpdateScroll(contactObj));
            contact.SetSubmitHandler(() => OnClickContact(contact, character, accessory));

            // 후에 파괴를 위한 리스트에 추가
            contactList.Add(contactObj);

            // 해당 캐릭터가 들고 있는 악세사리를 먼저, 없다면 가장 첫 악세사리를 먼저 선택
            bool isEquipped = IsEquip(character, accessory);
            if (firstSelect == null || isEquipped)
            {
                firstSelect = contactObj;

                // 캐릭터가 들고 있는 장비는 저장해놓기
                if (isEquipped)
                    currentEquip = contact;
            }
        }

        // 목록 사이즈 업데이트
        ContentResize();

        // 기본 사이즈 설정
        originContentSize = content.rect.height;

        // 각 오브젝트 위치 저장
        UpdateContactPos();

        // 버튼 네비게이션 설정
        naviGroup.SetupChildsNavigation();
    }

    /// <summary>
    /// 누군가 해당 장비를 착용하고 있는 지 여부를 리턴
    /// </summary>
    /// <param name="accessory">확인하고자 하는 장비</param>
    /// <returns></returns>
    private bool IsEquipAnyone(Accessory accessory)
    {
        // 언락된 캐릭터만 뽑아내기
        List<CharacterData> unlockChrs = PartyData.Instance.Characters.Where(chr => chr.IsUnlocked).ToList();

        foreach (CharacterData chr in unlockChrs)
        {
            // 언락된 캐릭터들에 한에서만 착용 중인 지 체크
            if (chr.Accessory1 == accessory || chr.Accessory2 == accessory) return true;
        }

        return false;
    }

    private void UpdateScroll(GameObject focusContact)
    {
        // contact 사이즈 변동을 고려하여 스크롤 사이즈 업데이트
        float detailSize = focusContact.GetComponent<EquipContact>().DetailSize;
        float originSize = focusContact.GetComponent<RectTransform>().rect.height;
        float addSize = detailSize - originSize;

        // 스크롤 사이즈 적용
        content.sizeDelta = new Vector2(0, originContentSize + addSize);

        // 스크롤 위치 업데이트
        UpdateScrollPosition(focusContact);
    }

    protected override void UpdateScrollPosition(GameObject focusContact)
    {
        float contactY = contactsY[focusContact];

        // 화면에 보여지는 최소 최대 y값
        float minY = -content.localPosition.y - viewportRect.rect.height + layoutGroup.padding.bottom;
        float maxY = -content.localPosition.y - layoutGroup.padding.top;

        // 해당 연락처 오브젝트의 최하단 및 최상단 y값
        float contactMinY = contactY - focusContact.GetComponent<EquipContact>().DetailSize;
        float contactMaxY = contactY;

        // 화면에 오브젝트가 일부라도 잘리는 지 판단
        float endValue = contactY;
        if (contactMinY < minY)
        {
            // 하단이 잘렸다면 잘려나가는 부분만큼 스크롤을 위로 올리기
            endValue = content.localPosition.y + minY - contactMinY;
        }
        else if (contactMaxY > maxY)
        {
            // 상단이 잘렸다면 잘려나가는 부분만큼 스크롤을 아래로 내리기
            endValue = content.localPosition.y - contactMaxY + maxY;
        }

        // 잘려나간 부분이 나오도록 스크롤 조정
        if (endValue != contactY)
        {
            content.DOLocalMoveY(endValue, 0.2f);
        }
    }

    private void OnClickContact(EquipContact contact, CharacterData character, Accessory selectItem)
    {
        // 장비 장착
        EquipItem(character, selectItem);

        // 이전 장비 장착 마크 해제
        currentEquip?.SetEquipMark(false);

        // 현재 장비 업데이트
        currentEquip = contact;

        // 현재 장비 장착 마크 설정
        currentEquip.SetEquipMark(true);

        // 장비 교체 후 알림
        GameEventManager.Instance.NotifyEquipUpdate();
    }

    private void EquipItem(CharacterData character, Accessory selectItem)
    {
        if (slot == AccessorySlot.Accessory1)
            character.Accessory1 = selectItem;
        else
            character.Accessory2 = selectItem;
    }

    private bool IsEquip(CharacterData chr, Accessory accessory)
    {
        if (slot == AccessorySlot.Accessory1)
            return chr.Accessory1 == accessory;
        else
            return chr.Accessory2 = accessory;
    }

    private void ContentResize()
    {
        float height = layoutGroup.padding.bottom + layoutGroup.padding.top;
        int count = content.childCount;

        for (int i = 0; i < count; i++)
        {
            RectTransform child = content.GetChild(i) as RectTransform;

            // 비활성화 된 오브젝트는 계산에서 제외
            if (!child.gameObject.activeSelf) continue;

            height += child.rect.height;

            // 마지막 요소를 제외하고서 spacing 계산
            if (i < count - 1) height += layoutGroup.spacing;
        }

        // 사이즈 반영
        content.sizeDelta = new Vector2(content.sizeDelta.x, height);
    }

    private void UpdateContactPos()
    {
        float contactHeight = contactPrefab.GetComponent<RectTransform>().rect.height;
        float posY = -layoutGroup.padding.top;

        // 각 항목들의 본래 위치 저장해두기
        foreach (GameObject contact in contactList)
        {
            contactsY[contact] = posY;

            posY -= contactHeight + layoutGroup.spacing;
        }
    }

    protected override IEnumerator OpenAnimation()
    {
        // 모든 악세사리 목록을 불러오는 애니메이션이 완료 될 때까지 키 입력 금지
        ControlContext.Instance.KeyLock();

        yield return StartCoroutine(base.OpenAnimation());

        // 목록을 다 불러온 후 처음으로 선택할 악세사리 설정
        if (firstSelect != null)
            EventSystem.current.SetSelectedGameObject(firstSelect);

        // 키 입력 해제
        ControlContext.Instance.KeyUnlock();
    }
}