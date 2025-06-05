using System.IO;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("참조 데이터")]
    [SerializeField] private GameData gameData;

    [Header("Map")]
    public bool player;
    public bool ui;
    public bool battle;

    private void Update()
    {
        ControlContext controller = ControlContext.Instance;

        player = controller.KeyInput.Player.enabled;
        ui = controller.KeyInput.UI.enabled;
        battle = controller.KeyInput.Battle.enabled;
    }

    private void Awake()
    {
        // 총 스크립트 개수
        string[] files = Directory.GetFiles("Assets/Scripts", "*.cs", SearchOption.AllDirectories);
        int totalLines = 0;

        foreach (string file in files)
        {
            int lineCount = File.ReadAllLines(file).Length;
            totalLines += lineCount;
        }

        Debug.Log($"Total Scripts: {files.Count()}, Total Lines: {totalLines}");

        // 게임 시작 전 설정
        StartGame();
    }

    public void StartGame()
    {
        // 시나리오 불러오기
        LoadScript(gameData.Chapter);

        // 플레이어 위치 초기화
        gameData.Position = new Vector2(0, 0);
    }

    private void LoadScript(Chapter data)
    {
        int chapter = data.ChapterNum;
        int root = data.RootNum;
        int subChapter = data.SubChapterNum;

        TextScriptResource.Instance.LoadScript(chapter, root, subChapter);
    }
}