using UnityEngine;
using System.IO;
using System.Collections.Generic;
using Endless.GameData;
using UnityEngine.SceneManagement;
using System.Linq;


#if UNITY_EDITOR
using UnityEditor;
#endif

public class SaveLoadManager : ScriptableObject
{
    // 저장 파일 위치
    private const string FILE_DIRECTORY = "Assets/Resources";
    private const string FILE_PATH = "Assets/Resources/SaveLoadManager.asset";

    private static SaveLoadManager _instance;
    public static SaveLoadManager Instance
    {
        get
        {
            if (_instance != null) return _instance;

            _instance = Resources.Load<SaveLoadManager>("/SaveLoadManager");

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
                _instance = AssetDatabase.LoadAssetAtPath<SaveLoadManager>(FILE_PATH);
                if (_instance == null)
                {
                    _instance = CreateInstance<SaveLoadManager>();
                    AssetDatabase.CreateAsset(_instance, FILE_PATH);
                }
            }
#endif
            return _instance;
        }
    }

    [Header("저장 데이터")]
    [SerializeField] private PartyData partyData;

    /************************************************************
    * [게임 데이터 저장]
    * 
    * 현재 진행 상황을 저장
    ************************************************************/

    public SaveData SaveGameData(string path)
    {
        SaveData data = GetCurrentData();

        // Data to Json
        string jsonData = JsonUtility.ToJson(data);
        string encryptData = Encrypt(jsonData);

        // File Write
        File.WriteAllText(path, encryptData);

        return data;
    }

    private SaveData GetCurrentData()
    {
        SaveData saveData = new SaveData();

        saveData.playerData = GetPlayerData();
        saveData.partyData = GetPartyData();
        saveData.storyData = GetStoryData();
        saveData.mapData = GetMapData();
        saveData.questData = GetQuestData();

        return saveData;
    }

    private SavePlayerData GetPlayerData()
    {
        SavePlayerData data = new SavePlayerData();

        GameData playerData = GameData.Instance;

        data.pos = playerData.Position;
        data.ap = playerData.AP;

        return data;
    }

    private List<SaveMemberData> GetPartyData()
    {
        List<SaveMemberData> data = new List<SaveMemberData>();

        foreach (CharacterData member in PartyData.Instance.Characters)
        {
            SaveMemberData memberData = new SaveMemberData();

            memberData.name = member.Name;
            memberData.isUnlocked = member.IsUnlocked;
            memberData.isParty = member.IsParty;
            memberData.hp = member.Stats.HP;
            memberData.maxHP = member.Stats.MaxHP;
            memberData.str = member.Stats.STR;
            memberData.agi = member.Stats.AGI;
            memberData.def = member.Stats.DEF;
            memberData.mp = member.Stats.MP;
            memberData.maxMP = member.Stats.MaxMP;
            memberData.sp = member.Stats.SP;
            memberData.maxSP = member.Stats.MaxSP;
            memberData.SAN = member.Stats.SAN;

            data.Add(memberData);
        }

        return data;
    }

    private SaveStoryData GetStoryData()
    {
        SaveStoryData data = new SaveStoryData();

        data.date = GameData.Instance.Date.ToString();

        Chapter chapter = GameData.Instance.Chapter;
        data.chapter = chapter.ChapterNum;
        data.root = chapter.RootNum;
        data.subChapter = chapter.SubChapterNum;

        return data;
    }

    private SaveMapData GetMapData()
    {
        SaveMapData data = new SaveMapData();

        data.name = GameData.Instance.MapName;
        data.scene = GameData.Instance.MapScene;
        data.areas = GameData.Instance.AreaDatas.ToList();

        return data;
    }

    private SaveQuestData GetQuestData()
    {
        SaveQuestData data = new SaveQuestData();

        data.id = GameData.Instance.MainQuest.ID;
        data.title = GameData.Instance.MainQuest.Title;

        return data;
    }

    private string Encrypt(string data)
    {
        // json 데이터 암호화

        return data;
    }

    /************************************************************
    * [게임 데이터 불러오기]
    * 
    * 현재 진행 상황에 세이브 데이터를 불러오기
    ************************************************************/

    public SaveData ReadSaveFile(string path)
    {
        // File Read
        string readFileStr = File.ReadAllText(path);

        // Json to Data
        string decryptData = Decrypt(readFileStr);
        SaveData data = JsonUtility.FromJson<SaveData>(decryptData);

        return data;
    }

    private string Decrypt(string data)
    {
        // 파일 복호화

        return data;
    }

    public void LoadGameData(SaveData data)
    {
        SetPlayerData(data.playerData);
        SetPartyData(data.partyData);
        SetStoryData(data.storyData);
        SetMapData(data.mapData);
        SetQuestData(data.questData);

        // 세이브 데이터 로드로 인해 변수가 바뀌었음을 알림
        GameEventManager.Instance.NotifyDataLoaded();
    }

    private void SetPlayerData(SavePlayerData data)
    {
        GameData.Instance.Position = data.pos;
        GameData.Instance.AP = data.ap;
    }

    private void SetPartyData(List<SaveMemberData> data)
    {
        foreach (SaveMemberData memberData in data)
        {
            CharacterData characterData = partyData.GetCharacter(memberData.name);

            characterData.IsUnlocked = memberData.isUnlocked;
            characterData.IsParty = memberData.isParty;
            characterData.Stats.HP = memberData.hp;
            characterData.Stats.MaxHP = memberData.maxHP;
            characterData.Stats.STR = memberData.str;
            characterData.Stats.AGI = memberData.agi;
            characterData.Stats.DEF = memberData.def;
            characterData.Stats.MP = memberData.mp;
            characterData.Stats.MaxMP = memberData.maxMP;
            characterData.Stats.SP = memberData.sp;
            characterData.Stats.MaxSP = memberData.maxSP;
            characterData.Stats.SAN = memberData.SAN;
        }
    }

    private void SetStoryData(SaveStoryData data)
    {
        GameData.Instance.Date = Date.StrToDate(data.date);
        GameData.Instance.Chapter = new Chapter(data.chapter, data.root, data.subChapter);
    }

    private void SetMapData(SaveMapData data)
    {
        GameData.Instance.MapName = data.name;
        GameData.Instance.MapScene = data.scene;
        GameData.Instance.AreaDatas = data.areas.ToHashSet();
    }

    private void SetQuestData(SaveQuestData data)
    {
        QuestData quest = QuestManager.Instance.FindQuest(data.id);

        GameData.Instance.MainQuest = quest;
    }

    public void LoadSaveFile(SaveData data)
    {
        // 회귀 여부에 따라 로딩 연출을 다르게 함
        LoadingScreen screen = IsRequireReturn(data) ? LoadingScreen.ClockLoading : LoadingScreen.Loading;

        // 로딩 과정에서 데이터 불러오기
        SceneLoadManager.onLoaded += () => LoadGameData(data);
        SceneLoadManager.LoadFieldScene(data.mapData.scene, UnloadSceneOptions.None, SceneFadeEffect.BlurFadeOut, SceneFadeEffect.BlurFadeIn, screen);
    }

    private bool IsRequireReturn(SaveData data)
    {
        // 현재 데이터와 불러올 데이터를 대조하여 회귀할 필요가 있는지 판단
        // #지금 단계에선 시간대가 과거인지만 판단
        Date loadDate = Date.StrToDate(data.storyData.date);
        return loadDate < GameData.Instance.Date;
    }
}