using System.Collections.Generic;

public class ContactApp : App
{
    private Dictionary<CharacterData, Contact> memberContacts = new();

#if UNITY_EDITOR
    private void OnValidate()
    {
        ui = GetComponent<ContactUI>();
    }
#endif

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
        contactUI.SetPlayerContact(PartyData.Instance.Player);

        // 파티 편입이 가능한 캐릭터 수만큼 오브젝트 생성
        foreach (CharacterData character in PartyData.Instance.Characters)
        {
            if (character is PlayerData) continue;

            if (memberContacts.ContainsKey(character)) // 기존 오브젝트가 있다면 정보만 갱신
                memberContacts[character].SetInfo(character);
            else    // 없다면 새로 만들기
                memberContacts.Add(character, contactUI.CreateContact(character));
        }
    }
}