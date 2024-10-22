using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PartyData : ScriptableObject
{
    // 저장 파일 위치
    private const string FILE_DIRECTORY = "Assets/Resources/Object/Character";
    private const string FILE_PATH = "Assets/Resources/Object/Character/PartyData.asset";

    private static PartyData _instance;
    public static PartyData Instance
    {
        get
        {
            if (_instance != null) return _instance;

            _instance = Resources.Load<PartyData>("Object/Character/PartyData");

#if UNITY_EDITOR
            if (_instance == null)
            {
                // 파일 경로가 없을 경우 폴더 생성
                if (!AssetDatabase.IsValidFolder(FILE_DIRECTORY))
                {
                    string[] folders = FILE_DIRECTORY.Split('/');
                    string currentPath = folders[0];

                    for (int i = 1; i < folders.Length; i++)
                    {
                        if (!AssetDatabase.IsValidFolder(currentPath + "/" + folders[i]))
                        {
                            AssetDatabase.CreateFolder(currentPath, folders[i]);
                        }

                        currentPath += "/" + folders[i];
                    }
                }

                // Resource.Load가 실패했을 경우
                _instance = AssetDatabase.LoadAssetAtPath<PartyData>(FILE_PATH);

                if (_instance == null)
                {
                    _instance = CreateInstance<PartyData>();
                    AssetDatabase.CreateAsset(_instance, FILE_PATH);
                }
            }
#endif
            return _instance;
        }
    }

    [Header("게임 내 캐릭터 정보")]
    [SerializeField]
    private CharacterData _player;
    public CharacterData Player
    {
        get { return _player; }
    }

    [SerializeField]
    private List<CharacterData> _allMemberList;
    private Dictionary<string, CharacterData> allMemberDic;
    public List<CharacterData> AllMemberList
    {
        get { return _allMemberList; }
    }

    private void OnEnable()
    {
        List2Dic();
    }

    private void List2Dic()
    {
        allMemberDic = new Dictionary<string, CharacterData>();

        foreach (CharacterData member in _allMemberList)
        {
            allMemberDic[member.Name] = member;
        }
    }

    public CharacterData GetCharacter(string name)
    {
        return allMemberDic[name];
    }

    public void AddMember(string name)
    {
        CharacterData addMember = allMemberDic[name];

        addMember.IsUnlocked = true;
    }

    public void RemoveMember(string name)
    {
        CharacterData removeMember = allMemberDic[name];

        removeMember.IsUnlocked = false;
    }

    public List<CharacterData> GetPartyMembers()
    {
        List<CharacterData> partyList = new List<CharacterData>();

        foreach (CharacterData member in AllMemberList)
        {
            if (member.IsParty)
            {
                partyList.Add(member);
            }
        }

        return partyList;
    }

    public void AddParty(string name)
    {
        CharacterData addMember = allMemberDic[name];

        if (addMember.IsUnlocked == false)
        {
            // 파티 편성 가능 맴버에 없다면 추가
            AddMember(name);
        }

        addMember.IsParty = true;
    }

    public void RemoveParty(string name)
    {
        CharacterData removeMember = allMemberDic[name];

        removeMember.IsParty = false;
    }
}