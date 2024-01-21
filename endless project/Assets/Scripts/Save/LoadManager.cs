using Endless.GameData;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadManager : MonoBehaviour
{
    [Header("저장 데이터")]
    [SerializeField] private PlayerData playerData;
    [SerializeField] private GameData gameData;

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
        SetMapData(data.mapData);
        SetPlayerData(data.playerData);
        SetStoryData(data.storyData);
        SetQuestData(data.questData);
    }

    private void SetPlayerData(SavePlayerData data)
    {
        playerData.Position = data.pos;
        playerData.HP = data.hp;
        playerData.AP = data.ap;
        playerData.STR = data.str;
        playerData.AGI = data.agi;
        playerData.DEF = data.def;
        playerData.MP = data.mp;
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
    }

    private void SetQuestData(SaveQuestData data)
    {
        QuestData quest = QuestManager.FindQuest(data.id);

        gameData.MainQuest = quest;
    }
}