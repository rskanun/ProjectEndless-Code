using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TeamContactWindow : ContactWindow
{
    [Header("연락처 오브젝트")]
    [SerializeField] private List<Image> fadeOutObjects;
    [SerializeField] private GameObject contactPrefab;
    [SerializeField] private Transform contactTrans;

    [Header("참조 스크립트")]
    [SerializeField] private Diary diary;
    [SerializeField] private TeamContact playerContact;

    // 생성된 오브젝트 목록
    private Dictionary<CharacterData, TeamContact> memberContacts = new();

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

    protected override void InitContact()
    {
        // 플레이어 데이터 설정
        playerContact.UpdateInfo(PartyData.Instance.Player);

        // 파티 편입이 가능한 캐릭터 수만큼 오브젝트 생성
        foreach (CharacterData character in PartyData.Instance.Characters)
        {
            if (character is PlayerData) continue;

            if (memberContacts.ContainsKey(character)) // 기존 오브젝트가 있다면 정보만 갱신
                memberContacts[character].UpdateInfo(character);
            else // 없다면 새로 만들기
                memberContacts.Add(character, CreateContact(character));
        }

        // 플레이어의 연락처를 먼저 선택
        EventSystem.current.SetSelectedGameObject(playerContact.gameObject);
    }

    private TeamContact CreateContact(CharacterData character)
    {
        GameObject contactObj = Instantiate(contactPrefab, contactTrans);
        TeamContact contact = contactObj.GetComponent<TeamContact>();

        contact.UpdateInfo(character);
        contact.SetSelectAction(() => diary.UpdateDiary(character));

        return contact;
    }
}