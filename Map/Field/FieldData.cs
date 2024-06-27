using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FieldData : ScriptableObject
{
    // 저장 파일 위치
    private const string OPTION_FILE_DIRECTORY = "Assets/Resources";
    private const string FILE_DIRECTORY = "Assets/Resources/InGameData";
    private const string FILE_PATH = "Assets/Resources/InGameData/FieldData.asset";

    private static FieldData _instance;
    public static FieldData Instance
    {
        get
        {
            if (_instance != null) return _instance;

            _instance = Resources.Load<FieldData>("InGameData/FieldData");

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

                    AssetDatabase.CreateFolder("Assets/Resources", "InGameData");
                }

                // Resource.Load가 실패했을 경우
                _instance = AssetDatabase.LoadAssetAtPath<FieldData>(FILE_PATH);

                if (_instance == null)
                {
                    _instance = CreateInstance<FieldData>();
                    AssetDatabase.CreateAsset(_instance, FILE_PATH);
                }
            }
#endif

            return _instance;
        }
    }

    [Header("이벤트")]
    [SerializeField] private GameEvent fieldEvent;

    private Tilemap _currentField;
    public Tilemap CurrentField
    {
        get { return _currentField; }
        set
        { 
            _currentField = value;

            fieldEvent.NotifyUpdate();
        }
    }
}