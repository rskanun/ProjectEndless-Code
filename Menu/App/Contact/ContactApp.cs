using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ContactApp : App
{
    // 참조 스크립트
    [SerializeField] private Diary diary;
    [SerializeField] private Contact playerContact;

    private Dictionary<CharacterData, Contact> memberContacts = new();

#if UNITY_EDITOR
    private void OnValidate()
    {
        ui = GetComponent<ContactUI>();
    }
#endif

    private void Awake()
    {
        playerContact.SetSelectAction(() => diary.UpdateDiary(PartyData.Instance.Player));
    }

    private void OnDestroy()
    {
        // 생성된 오브젝트 모두 파괴
        foreach (Contact contact in memberContacts.Values)
        {
            Destroy(contact.gameObject);
        }
    }

    protected override void OnOpened()
    {
        if (ui is not ContactUI contactUI) return;

        // 플레이어 데이터 설정
        playerContact.UpdateInfo(PartyData.Instance.Player);

        // 파티 편입이 가능한 캐릭터 수만큼 오브젝트 생성
        foreach (CharacterData character in PartyData.Instance.Characters)
        {
            if (character is PlayerData) continue;

            if (memberContacts.ContainsKey(character)) // 기존 오브젝트가 있다면 정보만 갱신
                memberContacts[character].UpdateInfo(character);
            else    // 없다면 새로 만들기
                memberContacts.Add(character, contactUI.CreateContact(character, () => diary.UpdateDiary(character)));
        }

        // 플레이어의 연락처를 먼저 선택
        EventSystem.current.SetSelectedGameObject(playerContact.gameObject);
    }
}