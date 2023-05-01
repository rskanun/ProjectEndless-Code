using Assets.Script.System.Menu.Save;
using Assets.Script.UI.Menu;
using Assets.Script.UI.Menu.Save;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

[Serializable]
public class SaveData
{
    // Player Data
    public Vector2 pos;
    public int hp;
    public int maxHP;
    public int ap;
    public int maxAP;
    public int str;
    public int agi;
    public int def;
    public int mp;
    public int armorPen;

    // Date
    public string date = DateTime.Now.ToString("MMdd");

    // Quest
    public string location = "테스트 월드";
    public string quest = "게임 완성";
}

public class SaveManager : MonoBehaviour
{
    private string fileExtension = ".dat";
    private string fileName = "_DiaryPage";
    private string path
    {
        get
        {
            string _path = Path.Combine(Application.persistentDataPath, "SaveFile");

            // 경로상 파일 검사
            if (!Directory.Exists(_path))
            {
                Directory.CreateDirectory(_path);
            }

            return _path;
        }
    }

    private int maxFileNum = 0;
    private List<SaveFileData> saveFiles;

    [Header("저장 데이터")]
    [SerializeField] private GameObject player;
    [SerializeField] private PlayerData playerData;
    [Header("참조 스크립트")]
    [SerializeField] private SaveUI ui;

    private void OnEnable()
    {
        saveFiles = new List<SaveFileData>();

        // 세이브 파일 오브젝트 초기 설정
        initSaveFileObj();
    }

    private void OnDisable()
    {
        saveFiles = null;
    }

    public void initSaveFileObj()
    {
        DirectoryInfo di = new DirectoryInfo(path);
        foreach (FileInfo file in di.GetFiles())
        {
            string fileNumStr = file.Name.Split('_')[0];
            int fileNum = int.Parse(fileNumStr);

            // 최신 파일 넘버 수정
            if(maxFileNum < fileNum)
            {
                maxFileNum = fileNum;
            }

            string str = File.ReadAllText(file.ToString());
            SaveData data = JsonUtility.FromJson<SaveData>(str);

            saveFiles.Add(createSaveFileData(data, fileNum));
        }

        ui.initSaveFileObj(saveFiles);
    }

    public void addSaveData()
    {
        string name = (++maxFileNum) + fileName + fileExtension;

        // 데이터 json 변환
        string filePath = Path.Combine(path, name);
        SaveData data = savePlayerData(filePath);

        SaveFileData saveFileData = createSaveFileData(data, maxFileNum);
        saveFiles.Add(saveFileData);
        ui.addSaveFileObj(saveFileData);

        Alert.makeMsg("데이터 기록이 완료되었습니다!").show();
    }

    public void rewriteSaveData(int index)
    {
        string name = index + fileName + fileExtension;

        // 데이터 json 변환
        string filePath = Path.Combine(path, name);
        SaveData data = savePlayerData(filePath);

        SaveFileData saveFileData = createSaveFileData(data, index);
        saveFiles.Add(saveFileData);
        ui.reloadSaveFileObj(saveFileData);

        Alert.makeMsg("데이터 기록이 완료되었습니다!").show();
    }

    private SaveData savePlayerData(string filePath)
    {
        SaveData saveData = new SaveData();

        saveData.pos = player.transform.position;
        saveData.hp = playerData.HP;
        saveData.maxHP = playerData.MaxHP;
        saveData.ap = playerData.AP;
        saveData.maxAP = playerData.MaxAP;
        saveData.str = playerData.STR;
        saveData.agi = playerData.Speed;
        saveData.def = playerData.DEF;
        saveData.mp = playerData.MP;
        saveData.armorPen = playerData.ArmorPenetration;

        string jsonData = JsonUtility.ToJson(saveData);
        File.WriteAllText(filePath, jsonData);

        return saveData;
    }

    private SaveFileData createSaveFileData(SaveData data, int id)
    {
        SaveFileData saveFileData = new SaveFileData();
        saveFileData.Data = data;
        saveFileData.Id = id;

        return saveFileData;
    }

    public void loadData(int index)
    {

    }
}