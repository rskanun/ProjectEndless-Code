using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;



#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class SaveData
{
    [Header("플레이어 데이터")]
    public SavePlayerData playerData;

    [Header("파티 데이터")]
    public List<SaveMemberData> partyData;

    [Header("인게임 진행 상황")]
    public SaveStoryData storyData;

    [Header("맵 데이터")]
    public SaveMapData mapData;

    [Header("퀘스트 진행 상황")]
    public SaveQuestData questData;
}

[System.Serializable]
public struct SavePlayerData
{
    public Vector2 pos;
    public int ap;
}

[System.Serializable]
public struct SaveMemberData
{
    public string name;
    public bool isUnlocked;
    public bool isParty;
    public int hp;
    public int maxHP;
    public int str;
    public int agi;
    public int def;
    public int mp;
    public int maxMP;
    public int sp;
    public int maxSP;
    public int SAN;
}

[System.Serializable]
public struct SaveStoryData
{
    public string date;
    public int chapter;
    public int root;
    public int subChapter;
}

[System.Serializable]
public struct SaveMapData
{
    public string id;
    public string name;
}

[System.Serializable]
public struct SaveQuestData
{
    public int id;
    public string title;
}

public class SaveFileInfo : ScriptableObject
{
    private const string DIALOG_FILE_DIRECTORY = "Assets/Resources";
    private const string FILE_DIRECTORY = "Assets/Resources/Option";
    private const string FILE_PATH = "Assets/Resources/Option/SaveFileInfo.asset";

    private static SaveFileInfo _instance;
    public static SaveFileInfo Instance
    {
        get
        {
            if (_instance != null) return _instance;

            _instance = Resources.Load<SaveFileInfo>("Option/SaveFileInfo");

#if UNITY_EDITOR
            if (_instance == null)
            {
                // 파일 경로가 없을 경우 폴더 생성
                if (!AssetDatabase.IsValidFolder(FILE_DIRECTORY))
                {
                    if (!AssetDatabase.IsValidFolder(DIALOG_FILE_DIRECTORY))
                    {
                        AssetDatabase.CreateFolder("Assets", "Resources");
                    }

                    AssetDatabase.CreateFolder("Assets/Resources", "Option");
                }

                // Resource.Load가 실패했을 경우
                _instance = AssetDatabase.LoadAssetAtPath<SaveFileInfo>(FILE_PATH);

                if (_instance == null)
                {
                    _instance = CreateInstance<SaveFileInfo>();
                    AssetDatabase.CreateAsset(_instance, FILE_PATH);
                }
            }
#endif

            return _instance;
        }
    }

    [SerializeField] private string fileExtension = ".dat";
    [SerializeField] private string fileName = "DiaryPage";

    private string _filePath;
    public string FilePath
    {
        get
        {
            if (string.IsNullOrEmpty(_filePath))
            {
                _filePath = Path.Combine(Application.persistentDataPath, "SaveFile");

                // 경로상 파일 검사
                if (!Directory.Exists(_filePath))
                {
                    Directory.CreateDirectory(_filePath);
                }
            }

            return _filePath;
        }
    }

    public string GetFileName(int id)
    {
        return fileName + id + fileExtension;
    }

    public int GetFileNum(string file)
    {
        string pattern = $@"{Regex.Escape(fileName)}(\d+){Regex.Escape(fileExtension)}";

        Match match = Regex.Match(file, pattern);
        if (match.Success)
        {
            // 추출된 숫자 부분을 정수로 변환하여 반환
            return int.Parse(match.Groups[1].Value);
        }
        else
        {
            // 형식에 맞지 않는 파일일 경우(숫자를 추출할 수 없는 경우)
            return -1;
        }
    }
}