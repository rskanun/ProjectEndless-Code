using Assets.Script.UI.Menu.App.Save;
using Assets.Script.UI.Menu.Save;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class SaveData
{
    // Player Data
    public Vector2 pos;
    public Vector2 angle;
    public int hp;
    public int maxHP;
    public int ap;
    public int maxAP;
    public int str;
    public int agi;
    public int def;
    public int mp;

    // Date
    public string date;

    // Quest
    public string location = "테스트 월드";
    public string quest = "게임 완성";
}

[Serializable]
public class Date
{
    public int year;
    public int month;

}

public class SaveManager : MonoBehaviour
{
    private string fileExtension = ".dat";
    private string fileName = "_DiaryPage";

    private string _path;
    private string Path
    {
        get
        {
            if(_path == null)
            {
                _path = System.IO.Path.Combine(Application.persistentDataPath, "SaveFile");

                // 경로상 파일 검사
                if (!Directory.Exists(_path))
                {
                    Directory.CreateDirectory(_path);
                }
            }

            return _path;
        }
    }

    // 파일 함수
    private int maxFileNum = 0;
    private Dictionary<int, SaveData> saveFiles = new Dictionary<int, SaveData>();

    [Header("저장 데이터")]
    [SerializeField] private GameObject player;
    [SerializeField] private PlayerData playerData;
    [Header("참조 스크립트")]
    [SerializeField] private SaveUI saveUI;
    [SerializeField] private LoadUI loadUI;

    public void initSave()
    {
        // 세이브 파일 오브젝트 초기 설정
        initSaveFile();

        saveUI.initSaveFileObj(saveFiles);
    }

    public void initLoad()
    {
        // 세이브 파일 오브젝트 초기 설정
        initSaveFile();

        loadUI.initSaveFileObj(saveFiles);
    }

    private void initSaveFile()
    {
        DirectoryInfo di = new DirectoryInfo(Path);
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

            saveFiles[fileNum] = data;
        }
    }

    private void OnDisable()
    {
        saveFiles.Clear();
        maxFileNum = 0;
    }

    /************************************************************
    * [세이브]
    * 
    * 현재 진행 상황을 저장
    ************************************************************/

    public void addSaveData()
    {
        string name = (++maxFileNum) + fileName + fileExtension;

        // 데이터 json 변환
        string filePath = System.IO.Path.Combine(Path, name);
        SaveData data = saveData(filePath);

        saveFiles[maxFileNum] = data;
        saveUI.addSaveFileObj(maxFileNum, data);

        Alert.makeMsg("데이터 기록이 완료되었습니다!").show();
    }

    public void rewriteSave(int id)
    {
        Debug.Log(id);
        Confirm.makeMsg("이미 저장된 내용이 있는 파일입니다. 그래도 덮어 씌우시겠습니까?", "계속", "취소")
        .setYesCallBack(() =>
        {
            rewriteSaveData(id);
        }).show();
    }

    private void rewriteSaveData(int id)
    {
        string name = id + fileName + fileExtension;

        // 데이터 json 변환
        string filePath = System.IO.Path.Combine(Path, name);
        SaveData data = saveData(filePath);

        saveFiles[maxFileNum] = data;
        saveUI.reloadSaveFileObj(id, data);

        Alert.makeMsg("데이터 기록이 완료되었습니다!").show();
    }

    private SaveData saveData(string filePath)
    {
        SaveData saveData = new SaveData();

        savePlayerData(saveData);
        saveOptionDate(saveData);

        // Data to Json
        string jsonData = JsonUtility.ToJson(saveData);
        File.WriteAllText(filePath, jsonData);

        return saveData;
    }

    private void savePlayerData(SaveData saveData)
    {
        saveData.pos = player.transform.position;
        saveData.hp = playerData.HP;
        saveData.ap = playerData.AP;
        saveData.str = playerData.STR;
        saveData.agi = playerData.AGI;
        saveData.def = playerData.DEF;
        saveData.mp = playerData.MP;
    }

    private void saveOptionDate(SaveData saveData)
    {
        saveData.date = OptionSetting.Instance.Date.ToString("O");
    }

    /************************************************************
    * [로드]
    * 
    * 현재 진행 상황에 세이브 데이터를 불러오기
    ************************************************************/

    public void loadData(int id)
    {
        SaveData data = saveFiles[id];

        loadPlayerData(data);
        loadOptionDate(data);
    }

    private void loadPlayerData(SaveData data)
    {
        player.transform.position = data.pos;
        playerData.HP = data.hp;
        playerData.AP = data.ap;
        playerData.STR = data.str;
        playerData.AGI = data.agi;
        playerData.DEF = data.def;
        playerData.MP = data.mp;
    }

    private void loadOptionDate(SaveData data)
    {
        OptionSetting.Instance.Date = DateTime.Parse(data.date);
    }    
}