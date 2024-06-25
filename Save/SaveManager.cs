using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private ReadOnlyGameData gameData;

    /************************************************************
    * [게임 데이터 저장]
    * 
    * 현재 진행 상황을 저장
    ************************************************************/

    public SaveData SaveGameData(string path)
    {
        gameData = ReadOnlyGameData.Instance;

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
        saveData.storyData = GetStoryData();
        saveData.mapData = GetMapData();
        saveData.questData = GetQuestData();

        return saveData;
    }

    private SavePlayerData GetPlayerData()
    {
        SavePlayerData data = new SavePlayerData();

        ReadOnlyPlayerData playerData = ReadOnlyPlayerData.Instance;

        data.pos = playerData.Position;
        data.ap = playerData.AP;

        return data;
    }

    private List<SaveMemberData> GetPartyData()
    {
        List<SaveMemberData> data = new List<SaveMemberData>();

        PartyData partyData = PartyData.Instance;
        foreach (CharacterData member in partyData.AllMemberList)
        {
            SaveMemberData memberData = new SaveMemberData();

            memberData.name = member.Name;
            memberData.isUnlocked = member.IsUnlocked;
            memberData.isParty = member.IsParty;
            memberData.hp = member.Stat.HP;
            memberData.maxSP = member.Stat.MaxSP;
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
}