using UnityEngine;
using Endless.GameData;
using UnityEngine.Tilemaps;
using System.Collections.Generic;


#if UNITY_EDITOR
using UnityEditor;
#endif

public enum GameState
{
    Title,
    Field,
    Battle
}

public class GameData : ScriptableObject
{
    // 저장 파일 위치
    private const string OPTION_FILE_DIRECTORY = "Assets/Resources";
    private const string FILE_DIRECTORY = "Assets/Resources/Option";
    private const string FILE_PATH = "Assets/Resources/Option/GameData.asset";

    private static GameData _instance;
    public static GameData Instance
    {
        get
        {
            if (_instance != null) return _instance;

            _instance = Resources.Load<GameData>("Option/GameData");

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

                    AssetDatabase.CreateFolder("Assets/Resources", "Option");
                }

                // Resource.Load가 실패했을 경우
                _instance = AssetDatabase.LoadAssetAtPath<GameData>(FILE_PATH);
                if (_instance == null)
                {
                    _instance = CreateInstance<GameData>();
                    AssetDatabase.CreateAsset(_instance, FILE_PATH);
                }
            }
#endif
            return _instance;
        }
    }

    [SerializeField]
    private GameState _state;
    public GameState State
    {
        get => _state;
        set
        {
            // 이전과 동일한 상태이면 무시
            if (_state == value) return;

            _state = value;

            // 상태 변경 알림 보내기
            GameEventManager.Instance.NotifyGameStateChanged();
        }
    }

    /************************************************************
    * [챕터 데이터]
    * 
    * 현재 플레이어가 진행 중인 챕터(1~9), 분기 번호, 챕터 내
    * 구간을 나눈 서브 챕터 번호 데이터
    ************************************************************/
    [SerializeField]
    private Chapter _chapter = new Chapter(9, 0, 0);
    public Chapter Chapter
    {
        get { return _chapter; }
        set { _chapter = value; }
    }

    /************************************************************
    * [날짜 데이터]
    * 
    * 현재 게임 내 날짜와 게임의 $&%와 관련된 데이터
    ************************************************************/
    [SerializeField]
    private Date _date = new Date(11, 19);
    public Date Date
    {
        get { return _date; }
        set { _date = value; }
    }

    [SerializeField]
    private RemainTime _time;
    public RemainTime Time
    {
        get
        {
            if (_time == null || _time.IsNull)
            {
                int loadTime = PlayerPrefs.GetInt("remainTime");

                if (loadTime <= 0) _time = new RemainTime(30227);
                else _time = new RemainTime(loadTime);
            }

            return _time;
        }
    }

    /************************************************************
    * [퀘스트 데이터]
    * 
    * 현재 플레이어가 진행 중인 퀘스트 관련 데이터
    ************************************************************/
    [SerializeField]
    private QuestData _questData;
    public QuestData MainQuest
    {
        get { return _questData; }
        set { _questData = value; }
    }

    /************************************************************
    * [위치 데이터]
    * 
    * 현재 플레이어가 있는 지형 및 위치 관련 데이터
    ************************************************************/
    [SerializeField]
    private MapData _mapData;
    public MapData MapData
    {
        get { return _mapData; }
        set { _mapData = value; }
    }

    private HashSet<AreaData> _areaDatas = new();
    public HashSet<AreaData> AreaDatas
    {
        get { return _areaDatas; }
        set { _areaDatas = value; }
    }

    [SerializeField]
    private Vector2 _pos;
    public Vector2 Position
    {
        get { return _pos; }
        set { _pos = value; }
    }

    /************************************************************
    * [각성치 데이터]
    * 
    * 주인공의 각성 수치로 시나리오에 벗어나는 행동을 할 시 올라간다.
     * 50% 달성 시 플레이어 제어권을 잃으며, 100%를 달성할 시
     * 강제 루프를 진행한다.
    ************************************************************/
    private readonly int MaxAP = 100;
    [SerializeField]
    private int _awakenPoint;
    public int AP
    {
        get { return _awakenPoint; }
        set
        {
            if (_awakenPoint != value)
            {
                // 입력값이 음수일 경우
                if (value < 0)
                    _awakenPoint = 0;
                // 입력값이 최대치를 초과한 경우
                else if (value > MaxAP)
                    _awakenPoint = MaxAP;
                else
                    _awakenPoint = value;
            }
        }
    }
}