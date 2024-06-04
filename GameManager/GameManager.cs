using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("참조 스크립터블 오브젝트")]
    [SerializeField] private GameData gameData;

    [Header("플레이어 데이터")]
    [SerializeField] private Player player;

    private ScriptResource scriptResource;
    private ControlContext controller;

    private static GameManager _instance;
    public static GameManager Instance
    {
        get { return _instance; }
    }

    private void Start()
    {
        if (_instance == null)
        {
            _instance = this;

            Init();
            DontDestroyOnLoad(gameObject);
        }
        else
            DestroyImmediate(gameObject);
    }

    private void Init()
    {
        scriptResource = ScriptResource.Instance;
        controller = ControlContext.Instance;

        controller.NoKeyDown = false;

        StartGame();
    }

    private void Update()
    {
        controller.OnKeyPressed();
    }

    public void StartGame()
    {
        LoadScript(gameData.Chapter);

        player.InitStat();
    }

    private void LoadScript(Chapter data)
    {
        int chapter = data.ChapterNum;
        int root = data.RootNum;
        int subChapter = data.SubChapterNum;

        scriptResource.LoadScript(chapter, root, subChapter);
    }
}