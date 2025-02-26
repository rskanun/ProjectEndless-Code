using UnityEngine;
using Endless.GameData;
using UnityEngine.Tilemaps;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ReadOnlyGameData : ScriptableObject
{
    // 저장 파일 위치
    private const string OPTION_FILE_DIRECTORY = "Assets/Resources";
    private const string FILE_DIRECTORY = "Assets/Resources/InGameData";
    private const string FILE_PATH = "Assets/Resources/InGameData/ReadOnlyGameData.asset";

    private static ReadOnlyGameData _instance;
    public static ReadOnlyGameData Instance
    {
        get
        {
            if (_instance != null) return _instance;

            _instance = Resources.Load<ReadOnlyGameData>("InGameData/ReadOnlyGameData");

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
                _instance = AssetDatabase.LoadAssetAtPath<ReadOnlyGameData>(FILE_PATH);

                if (_instance == null)
                {
                    _instance = CreateInstance<ReadOnlyGameData>();
                    AssetDatabase.CreateAsset(_instance, FILE_PATH);

                    // Player.asset 불러옴
                    GameData gameData = AssetDatabase.LoadAssetAtPath<GameData>(FILE_DIRECTORY + "/GameData.asset");

                    if (gameData == null)
                    {
                        gameData = CreateInstance<GameData>();
                        AssetDatabase.CreateAsset(gameData, FILE_DIRECTORY + "/GameData.asset");
                    }

                    _instance.gameData = gameData;
                }
            }
#endif
            return _instance;
        }
    }

    [SerializeField]
    private GameData gameData;

    public Chapter Chapter
        => gameData.Chapter;

    public Date Date
        => gameData.Date;

    public RemainTime Time
        => gameData.Time;

    public QuestData MainQuest
        => gameData.MainQuest;

    public MapData MapData
        => gameData.MapData;

    public Vector2 Position
        => gameData.Position;

    public int AP
        => gameData.AP;
}