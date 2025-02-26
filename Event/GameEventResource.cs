using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameEventResource : ScriptableObject
{
    // 저장 파일 위치
    private const string FILE_DIRECTORY = "Assets/Resources/GameEvent";
    private const string FILE_PATH = "Assets/Resources/GameEvent/GameEventResource.asset";

    private static GameEventResource _instance;
    public static GameEventResource Instance
    {
        get
        {
            if (_instance != null) return _instance;

            _instance = Resources.Load<GameEventResource>("GameEvent/GameEventResource");

#if UNITY_EDITOR
            if (_instance == null)
            {
                // 파일 경로가 없을 경우 폴더 생성
                if (!AssetDatabase.IsValidFolder(FILE_DIRECTORY))
                {
                    string[] folders = FILE_DIRECTORY.Split('/');
                    string currentPath = folders[0];

                    for (int i = 1; i < folders.Length; i++)
                    {
                        if (!AssetDatabase.IsValidFolder(currentPath + "/" + folders[i]))
                        {
                            AssetDatabase.CreateFolder(currentPath, folders[i]);
                        }
                        currentPath += "/" + folders[i];
                    }
                }

                // Resource.Load가 실패했을 경우
                _instance = AssetDatabase.LoadAssetAtPath<GameEventResource>(FILE_PATH);
                if (_instance == null)
                {
                    _instance = CreateInstance<GameEventResource>();
                    AssetDatabase.CreateAsset(_instance, FILE_PATH);
                }
            }
#endif
            return _instance;
        }
    }
    [Header("필드 이벤트")]
    [SerializeField]
    private GameEvent _areaMoveEvent;
    public GameEvent AreaMoveEvent
    {
        get { return _areaMoveEvent; }
    }

    [Header("전투 이벤트")]
    [SerializeField]
    private GameEvent _sequenceUpdateEvent;
    public GameEvent SequenceUpdateEvent
    {
        get { return _sequenceUpdateEvent; }
    }

    [SerializeField]
    private GameEvent _endTurnEvent;
    public GameEvent EndTurnEvent
    {
        get { return _endTurnEvent; }
    }

    [SerializeField]
    private GameEvent _startTurnEvent;
    public GameEvent StartTurnEvent
    {
        get { return _startTurnEvent; }
    }
}