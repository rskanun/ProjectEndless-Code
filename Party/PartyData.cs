using System.Collections.Generic;
using System.Linq;
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
    private PlayerData _player;
    public PlayerData Player
    {
        get { return _player; }
    }
    [SerializeField]
    private List<CharacterData> _members;
    private Dictionary<string, CharacterData> charactersDict;
    private List<CharacterData> _characters;
    public List<CharacterData> Characters => _characters;

#if UNITY_EDITOR
    private void OnValidate()
    {
        UpdateCharacterList();
        ChrList2Dict();
    }

    private void UpdateCharacterList()
    {
        _characters = new List<CharacterData>() { Player };
        foreach (CharacterData member in _members)
        {
            _characters.Add(member);
        }
    }

    private void ChrList2Dict()
    {
        charactersDict = new Dictionary<string, CharacterData>();

        // 인스펙터창을 통해 받은 맴버 리스트를 찾기 쉬운 딕셔너리로 변경
        foreach (CharacterData member in _characters)
        {
            charactersDict[member.Name] = member;
        }
    }
#endif

    public CharacterData GetCharacter(string name)
    {
        return charactersDict[name];
    }

    public List<CharacterData> GetPartyMembers()
    {
        return _characters.Where((chr) => chr.IsParty).ToList();
    }

    public void AddMember(string name)
    {
        CharacterData addMember = charactersDict[name];

        addMember.IsUnlocked = true;
    }

    public void RemoveMember(string name)
    {
        CharacterData removeMember = charactersDict[name];

        removeMember.IsUnlocked = false;
    }

    public void JoinParty(string name)
    {
        CharacterData addMember = charactersDict[name];

        addMember.IsParty = true;
    }

    public void KickParty(string name)
    {
        CharacterData removeMember = charactersDict[name];

        removeMember.IsParty = false;
    }
}