using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [Header("참조 데이터")]
    [SerializeField] private GameData gameData;
    [SerializeField] private PlayerData playerData;

    [Header("Map")]
    public bool player;
    public bool ui;
    public bool menu;
    public bool battle;

    private TextScriptResource scriptResource;
    private ControlContext controller;

    private static GameManager _instance;
    public static GameManager Instance
    {
        get { return _instance; }
    }

    private void Awake()
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
        scriptResource = TextScriptResource.Instance;
        controller = ControlContext.Instance;

        controller.Init();

        StartGame();
    }

    private void Update()
    {
        player = controller.KeyInput.Player.enabled;
        ui = controller.KeyInput.UI.enabled;
        menu = controller.KeyInput.Menu.enabled;
        battle = controller.KeyInput.Battle.enabled;
    }

    public void StartGame()
    {
        LoadScript(gameData.Chapter);

        playerData.Position = new Vector2(0, 0);
    }

    private void LoadScript(Chapter data)
    {
        int chapter = data.ChapterNum;
        int root = data.RootNum;
        int subChapter = data.SubChapterNum;

        scriptResource.LoadScript(chapter, root, subChapter);
    }
}