using Assets.Script.UI.Menu;
using Assets.Script.UI.Menu.Popup;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
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
    public int date = 0417;

    // Quest
    public string location = "테스트 월드";
    public string quest = "게임 완성";
}

public class SaveManager : MonoBehaviour
{
    private string fileExtension = ".dat";
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

    [Header("저장 데이터")]
    [SerializeField] private GameObject player;
    [SerializeField] private PlayerData playerData;
    [Header("참조 스크립트")]
    [SerializeField] private AlertUI alert;
    [SerializeField] private SaveUI ui;

    private void OnEnable()
    {
        // 세이브 파일 오브젝트 초기 설정
        initSaveFileObj();
    }

    private void initSaveFileObj()
    {
        Dictionary<string, SaveData> saveDic = new Dictionary<string, SaveData>();

        DirectoryInfo di = new DirectoryInfo(path);
        foreach (FileInfo file in di.GetFiles())
        {
            string str = File.ReadAllText(file.ToString());
            SaveData data = JsonUtility.FromJson<SaveData>(str);

            saveDic[file.Name] = data;
        }

        ui.setSaveFileObj(saveDic);
    }

    public void saveData(int index)
    {
        string fileName = index + "_DiaryPage" + fileExtension;

        // 데이터 json 변환
        string filePath = Path.Combine(path, fileName);
        savePlayerData(filePath);

        alert.setAlert("데이터 기록이 완료되었습니다!");
        alert.setActive(true);
    }

    private void savePlayerData(string filePath)
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
    }

    public void loadData(int index)
    {

    }
}