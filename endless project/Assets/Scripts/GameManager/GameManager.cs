using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("참조 스크립터블 오브젝트")]
    [SerializeField] private GameData gameData;

    [Header("플레이어 데이터")]
    [SerializeField] private Player player;

    private ScriptResource scriptResource;
    private ControlContext controller;

    private void Start()
    {
        scriptResource = ScriptResource.Instance;
        controller = ControlContext.Instance;

        StartGame();
    }
    private void Update()
    {
        controller.OnKeyPressed();
    }

    public void StartGame()
    {
        LoadScript();

        player.InitStat();
    }

    private void LoadScript()
    {
        int chapter = gameData.ChapterNum;
        int root = gameData.RootNum;
        int subChapter = gameData.SubChapterNum;

        scriptResource.LoadScript(chapter, root, subChapter);
    }
}