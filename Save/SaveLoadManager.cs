using UnityEngine;
using System.IO;
using System.Collections.Generic;
using Endless.GameData;
using UnityEngine.SceneManagement;

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
    [SerializeField] private GameData gameData;

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

        ReadOnlyGameData playerData = ReadOnlyGameData.Instance;

        data.pos = playerData.Position;
        data.ap = playerData.AP;

        return data;
    }

    private List<SaveMemberData> GetPartyData()
    {
        List<SaveMemberData> data = new List<SaveMemberData>();

        foreach (CharacterData member in PartyData.Instance.GetAllCharacters())
        {
            SaveMemberData memberData = new SaveMemberData();

            memberData.name = member.Name;
            memberData.isUnlocked = member.IsUnlocked;
            memberData.isParty = member.IsParty;
            memberData.hp = member.Stat.HP;
            memberData.maxHP = member.Stat.MaxHP;
            memberData.str = member.Stat.STR;
            memberData.agi = member.Stat.AGI;
            memberData.def = member.Stat.DEF;
            memberData.mp = member.Stat.MP;
            memberData.maxMP = member.Stat.MaxMP;
            memberData.sp = member.Stat.SP;
            memberData.maxSP = member.Stat.MaxSP;
            memberData.SAN = member.Stat.SAN;

            data.Add(memberData);
        }

        return data;
    }

    private SaveStoryData GetStoryData()
    {
        SaveStoryData data = new SaveStoryData();

        data.date = gameData.Date.ToString();

        Chapter chapter = gameData.Chapter;
        data.chapter = chapter.ChapterNum;
        data.root = chapter.RootNum;
        data.subChapter = chapter.SubChapterNum;

        return data;
    }

    private SaveMapData GetMapData()
    {
        SaveMapData data = new SaveMapData();

        data.id = gameData.MapData.ID;
        data.name = gameData.MapData.Name;
        data.currentArea = gameData.MapData.GetCurrentAreaID();
        data.areas = gameData.MapData.GetAreaDatas();

        return data;
    }

    private SaveQuestData GetQuestData()
    {
        SaveQuestData data = new SaveQuestData();

        data.id = gameData.MainQuest.ID;
        data.title = gameData.MainQuest.Title;

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
        GameEventResource.Instance.DataLoadEvent.NotifyUpdate();
    }

    private void SetPlayerData(SavePlayerData data)
    {
        gameData.Position = data.pos;
        gameData.AP = data.ap;
    }

    private void SetPartyData(List<SaveMemberData> data)
    {
        foreach (SaveMemberData memberData in data)
        {
            CharacterData characterData = partyData.GetCharacter(memberData.name);

            characterData.IsUnlocked = memberData.isUnlocked;
            characterData.IsParty = memberData.isParty;
            characterData.Stat.HP = memberData.hp;
            characterData.Stat.MaxHP = memberData.maxHP;
            characterData.Stat.STR = memberData.str;
            characterData.Stat.AGI = memberData.agi;
            characterData.Stat.DEF = memberData.def;
            characterData.Stat.MP = memberData.mp;
            characterData.Stat.MaxMP = memberData.maxMP;
            characterData.Stat.SP = memberData.sp;
            characterData.Stat.MaxSP = memberData.maxSP;
            characterData.Stat.SAN = memberData.SAN;
        }
    }

    private void SetStoryData(SaveStoryData data)
    {
        gameData.Date = Date.StrToDate(data.date);
        gameData.Chapter = new Chapter(data.chapter, data.root, data.subChapter);
    }

    private void SetMapData(SaveMapData data)
    {
        MapData map = MapManager.FindMap(data.id);

        gameData.MapData = map;
        gameData.MapData.SetCurrentArea(data.currentArea);
        gameData.MapData.SetAreaDatas(data.areas);
    }

    private void SetQuestData(SaveQuestData data)
    {
        QuestData quest = QuestManager.FindQuest(data.id);

        gameData.MainQuest = quest;
    }

    public void LoadSaveFile(SaveData data)
    {
        // 회귀 여부에 따라 로딩 연출을 다르게 함
        LoadingScreen screen = IsRequireReturn(data) ? LoadingScreen.ClockLoading : LoadingScreen.Loading;
        MapData map = MapManager.FindMap(data.mapData.id);

        // 로딩 과정에서 데이터 불러오기
        LoadSceneManager.loadingCallBack += () => LoadGameData(data);
        LoadSceneManager.Instance.LoadFieldScene(map.SceneName, UnloadSceneOptions.None, SceneFadeEffect.BlurFadeOut, SceneFadeEffect.BlurFadeIn, screen);
    }

    private bool IsRequireReturn(SaveData data)
    {
        // 현재 데이터와 불러올 데이터를 대조하여 회귀할 필요가 있는지 판단
        // #지금 단계에선 시간대가 과거인지만 판단
        Date loadDate = Date.StrToDate(data.storyData.date);
        return loadDate < ReadOnlyGameData.Instance.Date;
    }
}