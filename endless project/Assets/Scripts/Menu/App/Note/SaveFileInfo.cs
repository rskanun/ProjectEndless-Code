using UnityEngine;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class SaveData
{
    [Header("플레이어 데이터")]
    public SavePlayerData playerData;

    [Header("인게임 진행 상황")]
    public SaveStoryData storyData;

    [Header("맵 데이터")]
    public SaveMapData mapData;

    [Header("퀘스트 진행 상황")]
    public SaveQuestData questData;
}

public struct SavePlayerData
{
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
}

public struct SaveStoryData
{
    public string date;
    public int chapter;
    public int root;
    public int subChapter;
}

public struct SaveMapData
{
    public string id;
    public string name;
}

public struct SaveQuestData
{
    public int id;
    public string title;
}

public class SaveFileInfo : ScriptableObject
{
    // 저장 파일 위치
    private const string OPTION_FILE_DIRECTORY = "Assets/Resources";
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
                    if (!AssetDatabase.IsValidFolder(OPTION_FILE_DIRECTORY))
                    {
                        AssetDatabase.CreateFolder("Assets", "Resources");
                    }

                    AssetDatabase.CreateFolder(OPTION_FILE_DIRECTORY, "Option");
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

    private string fileExtension = ".dat";
    private string fileName = "DiaryPage";

    private string _filePath;
    public string FilePath
    {
        get
        {
            if (_filePath == null)
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
}