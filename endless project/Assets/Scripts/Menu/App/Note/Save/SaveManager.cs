using System;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    [Header("저장 데이터")]
    [SerializeField] private GameObject player;
    [SerializeField] private PlayerData playerData;
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

        saveData.playerData = GetCurrentPlayerData();
        saveData.storyData = GetCurrentStoryData();
        saveData.mapData = GetCurrentMapData();
        saveData.questData = GetCurrentQuestData();

        return saveData;
    }

    private SavePlayerData GetCurrentPlayerData()
    {
        SavePlayerData data = new SavePlayerData();

        data.pos = player.transform.position;
        data.hp = playerData.HP;
        data.ap = playerData.AP;
        data.str = playerData.STR;
        data.agi = playerData.AGI;
        data.def = playerData.DEF;
        data.mp = playerData.MP;

        return data;
    }

    private SaveStoryData GetCurrentStoryData()
    {
        SaveStoryData data = new SaveStoryData();
        
        DateTime date = OptionSetting.Instance.Date;
        data.date = OptionSetting.Instance.DateToStr(date);

        data.chapter = gameData.ChapterNum;
        data.root = gameData.RootNum;
        data.subChapter = gameData.SubChapterNum;

        return data;
    }

    private SaveMapData GetCurrentMapData()
    {
        SaveMapData data = new SaveMapData();

        data.id = gameData.MapData.ID;
        data.name = gameData.MapData.Name;

        return data;
    }

    private SaveQuestData GetCurrentQuestData()
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
}