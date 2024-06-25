using Endless.GameData;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LoadManager : MonoBehaviour
{
    [Header("저장 데이터")]
    [SerializeField] private PartyData partyData;
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
        SetPlayerData(data.playerData);
        SetPartyData(data.partyData);
        SetStoryData(data.storyData);
        SetMapData(data.mapData);
        SetQuestData(data.questData);
    }

    private void SetPlayerData(SavePlayerData data)
    {
        playerData.Position = data.pos;
        playerData.AP = data.ap;
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
    }

    private void SetQuestData(SaveQuestData data)
    {
        QuestData quest = QuestManager.FindQuest(data.id);

        gameData.MainQuest = quest;
    }
}