using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameEventManager : ScriptableObject
{
    // 저장 파일 위치
    private const string FILE_DIRECTORY = "Assets/Resources/GameEvent";
    private const string FILE_PATH = "Assets/Resources/GameEvent/GameEventManager.asset";

    private static GameEventManager _instance;
    public static GameEventManager Instance
    {
        get
        {
            if (_instance != null) return _instance;

            _instance = Resources.Load<GameEventManager>("GameEvent/GameEventManager");

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
                _instance = AssetDatabase.LoadAssetAtPath<GameEventManager>(FILE_PATH);
                if (_instance == null)
                {
                    _instance = CreateInstance<GameEventManager>();
                    AssetDatabase.CreateAsset(_instance, FILE_PATH);
                }
            }
#endif
            return _instance;
        }
    }
    [Header("공통 이벤트")]
    [SerializeField] private GameEvent _gameStateEvent;

    [Header("필드 이벤트")]
    [SerializeField] private GameEvent _areaMoveEvent;
    [SerializeField] private GameEvent _dataLoadEvent;
    [SerializeField] private GameEvent _fieldReturnEvent;
    [SerializeField] private GameEvent _equipUpdateEvent;
    [SerializeField] private GameEvent _statsUpdateEvent;

    [Header("전투 이벤트")]
    [SerializeField] private GameEvent _sequenceUpdateEvent;
    [SerializeField] private GameEvent _endTurnEvent;
    [SerializeField] private GameEvent _startTurnEvent;
    [SerializeField] private GameEvent _battleStartEvent;
    [SerializeField] private GameEvent _battleEndEvent;
    [SerializeField] private GameEvent _killEnemyEvent;
    [SerializeField] private GameEvent _parryingEvent;

    /// <summary>
    /// 게임 상태(타이틀, 필드, 전투) 변경 알림
    /// </summary>
    public void NotifyGameStateChanged()
    {
        _gameStateEvent.NotifyUpdate();
    }

    /************************************************************
     * [ 필드 이벤트 ]
     * 
     * 필드 내에서 일어나는 이벤트 알림
     ************************************************************/

    /// <summary>
    /// 플레이어가 맵의 구역을 이동하면 보내는 알림
    /// </summary>
    public void NotifyAreaChanged()
    {
        _areaMoveEvent.NotifyUpdate();
    }

    /// <summary>
    /// 세이브 파일 로드 시 보내는 알림
    /// </summary>
    public void NotifyDataLoaded()
    {
        _dataLoadEvent.NotifyUpdate();
    }

    /// <summary>
    /// 전투가 끝나 다시 필드로 돌아왔음을 알림
    /// </summary>
    public void NotifyFieldReturned()
    {
        _fieldReturnEvent.NotifyUpdate();
    }

    /// <summary>
    /// 캐릭터의 장비 변경 알림
    /// </summary>
    public void NotifyEquipUpdate()
    {
        _equipUpdateEvent.NotifyUpdate();
    }

    /// <summary>
    /// 파티 내 캐릭터들의 HP및 AP 스탯 변경 알림
    /// </summary>
    public void NotifyPartyStatsUpdate()
    {
        _statsUpdateEvent.NotifyUpdate();
    }

    /************************************************************
     * [ 전투 이벤트 ]
     * 
     * 전투 내에서 일어나는 이벤트 알림
     ************************************************************/

    /// <summary>
    /// 전투 순서 변경에 따른 알림
    /// </summary>
    public void NotifySequenceUpdate()
    {
        _sequenceUpdateEvent.NotifyUpdate();
    }

    /// <summary>
    /// 현재 차례인 엔티티의 턴 종료를 알림
    /// </summary>
    public void NotifyTurnEnded()
    {
        _endTurnEvent.NotifyUpdate();
    }

    /// <summary>
    /// 다음 차례인 엔티티의 턴 시작을 알림
    /// </summary>
    public void NotifyTurnStarted()
    {
        _startTurnEvent.NotifyUpdate();
    }

    /// <summary>
    /// 전투 시작을 알림
    /// </summary>
    public void NotifyBattleStarted()
    {
        _battleStartEvent.NotifyUpdate();
    }

    /// <summary>
    /// 전투가 끝났음을 알림
    /// </summary>
    public void NotifyBattleEnded()
    {
        _battleEndEvent.NotifyUpdate();
    }

    /// <summary>
    /// 전투 중 적을 처치했음을 알림
    /// </summary>
    public void NotifyEnemyDefeated()
    {
        _killEnemyEvent.NotifyUpdate();
    }

    /// <summary>
    /// 플레이어 진형 쪽 캐릭터가 적의 공격에 대해 패링에 성공했음을 알림
    /// </summary>
    public void NotifyParrySuccess()
    {
        _parryingEvent.NotifyUpdate();
    }
}