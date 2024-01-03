using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ScriptResource : ScriptableObject
{
    private const string DIALOG_FILE_DIRECTORY = "Assets/Resources";
    private const string FILE_DIRECTORY = "Assets/Resources/Scenario";
    private const string FILE_PATH = "Assets/Resources/Scenario/ScriptResource.asset";

    private static ScriptResource _instance;
    public static ScriptResource Instance
    {
        get
        {
            if (_instance != null) return _instance;

            _instance = Resources.Load<ScriptResource>("Scenario/ScriptResource");

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

                    AssetDatabase.CreateFolder("Assets/Resources", "Scenario");
                }

                // Resource.Load가 실패했을 경우
                _instance = AssetDatabase.LoadAssetAtPath<ScriptResource>(FILE_PATH);

                if (_instance == null)
                {
                    _instance = CreateInstance<ScriptResource>();
                    AssetDatabase.CreateAsset(_instance, FILE_PATH);
                }
            }
#endif

            return _instance;
        }
    }

    [MenuItem("GameObject/Singleton Scriptable Object/ScriptResource", false, 30)]
    public static void CreateInInspector()
    {
        ScriptResource instance = Instance;
    }

    private Script _currentScript;
    
    public Script CurrentScript
    {
        get { return _currentScript; }
    }

    public void LoadScript(int chapter, int root, int subChapter)
    {
        string path = GetFolderPath(chapter, root, subChapter);
        
        _currentScript = CsvReader.Instance.GetScript(path);
    }

    private string GetFolderPath(int chapter, int root, int subChapter)
    {
        // 챕터번호 1자리 + 분기번호 1자리 + 서브챕터번호 2자리
        string folderName = chapter.ToString() + root.ToString()
            + ((subChapter < 10) ? "0"  : "") + subChapter.ToString();

        string path = FILE_DIRECTORY + "/" + folderName;

        return path;
    }

    public bool HasLines(int id)
    {
        if (id > 0)
            return _currentScript.ContainsKey(id);

        // id값이 0보다 작으면 
        else return false;
    }
}