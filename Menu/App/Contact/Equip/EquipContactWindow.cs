using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class EquipContactWindow : ContactWindow
{
    [SerializeField] protected ContactApp app;
    [SerializeField] private NavigationGroup naviGroup;

    [Header("연락처 오브젝트")]
    [SerializeField] private GameObject contactPrefab;
    [SerializeField] private Transform contactTrans;

    private Dictionary<GameObject, float> contactsY = new();

    private GameObject firstSelect;
    private EquipContact currentEquip;
    private float originContentSize;

    public abstract ItemType EquipType { get; }

    private void OnDisable()
    {
        // 해당 창이 비활성화 될 때, 모든 오브젝트 목록을 지우기
        DestroyContactObjs();

        contactList.Clear();
        contactsY.Clear();

        // 스크롤 위치를 처음에 위치시키기
        content.localPosition = new Vector2(content.localPosition.x, 0);
    }

    protected override void InitContact()
    {
        // 플레이어가 가진 모든 무기 목록 띄우기
        foreach ((Item item, int count) in InventoryData.Instance.GetItems(EquipType))
        {
            Equip equip = item as Equip;
            CharacterData character = app.SelectCharacter;

            // 해당 칸에 선택된 캐릭터가 쓸 수 있는 무기 목록만 띄우기
            if (!IsEquipType(character, equip)) continue;

            // 해당 무기 정보를 토대로 한 연락처(=정보) 오브젝트 생성
            GameObject contactObj = Instantiate(contactPrefab, contactTrans);
            EquipContact contact = contactObj.GetComponent<EquipContact>();

            // 정보 및 핸들러 등록
            contact.UpdateInfo(equip, count, IsEquipAnyone(equip));
            contact.SetSelectAction(() => UpdateScrollPosition(contactObj));
            contact.SetSubmitHandler(() => OnClickContact(contact, character, equip));

            // 후에 파괴를 위한 리스트에 추가
            contactList.Add(contactObj);

            // 해당 캐릭터가 들고 있는 무기를 먼저, 없다면 가장 첫 무기를 먼저 선택
            bool isEquipped = IsEquip(character, equip);
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
    /// <param name="equip">확인하고자 하는 장비</param>
    /// <returns></returns>
    private bool IsEquipAnyone(Equip equip)
    {
        return PartyData.Instance.Characters
            .Where(chr => chr.IsUnlocked)
            .Any(chr =>
                chr.MainWeapon == equip
                || chr.OffWeapon == equip
                || chr.Accessory1 == equip
                || chr.Accessory2 == equip);
    }

    private void OnClickContact(EquipContact contact, CharacterData character, Equip selectEquip)
    {
        // 장비 장착
        EquipItem(character, selectEquip);

        // 이전 장비 장착 마크 해제
        currentEquip?.SetEquipMark(false);

        // 현재 장비 업데이트
        currentEquip = contact;

        // 현재 장비 장착 마크 설정
        currentEquip.SetEquipMark(true);

        // 장비 교체 후 알림
        GameEventManager.Instance.NotifyEquipUpdate();
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

    protected override void UpdateScrollPosition(GameObject focusContact)
    {
        // contact 사이즈 변동을 고려하여 스크롤 사이즈 업데이트
        float detailSize = focusContact.GetComponent<EquipContact>().DetailSize;
        float originSize = focusContact.GetComponent<RectTransform>().rect.height;
        float addSize = detailSize - originSize;

        // 스크롤 사이즈 적용
        content.sizeDelta = new Vector2(0, originContentSize + addSize);

        // 화면에 보여지는 최소 최대 y값
        float minY = -content.localPosition.y - viewportRect.rect.height + layoutGroup.padding.bottom;
        float maxY = -content.localPosition.y - layoutGroup.padding.top;

        // 해당 연락처 오브젝트의 최하단 및 최상단 y값
        float contactY = contactsY[focusContact];
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

    protected override IEnumerator OpenAnimation()
    {
        // 모든 무기 목록을 불러오는 애니메이션이 완료 될 때까지 키 입력 금지
        ControlContext.Instance.KeyLock();

        yield return StartCoroutine(base.OpenAnimation());

        // 목록을 다 불러온 후 처음으로 선택할 무기 설정
        if (firstSelect != null)
            EventSystem.current.SetSelectedGameObject(firstSelect);

        // 키 입력 해제
        ControlContext.Instance.KeyUnlock();
    }

    protected abstract bool IsEquipType(CharacterData character, Equip equip);
    protected abstract void EquipItem(CharacterData character, Equip equip);
    protected abstract bool IsEquip(CharacterData character, Equip equip);
}