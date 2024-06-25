using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("참조 데이터")]
    [SerializeField] private GameData gameData;
    [SerializeField] private PlayerData playerData;

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