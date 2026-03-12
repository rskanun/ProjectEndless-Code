using System.IO;
using System.Linq;
using Cysharp.Threading.Tasks;
using Endless.GameData;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization.Settings;

public class GameManager : MonoBehaviour
{
    [ReadOnly]
    public GameObject select;
    [SerializeField]
    private GameObject playerObj;
    [SerializeField]
    private GameObject trackingCamera;

    [Header("초기 설정")]
    [SerializeField] private GameState state;
    [SerializeField] private Chapter chapter;
    [SerializeField] private Vector2 pos;
    [SerializeField] private Date date;

    private void Update()
    {
        select = EventSystem.current.currentSelectedGameObject;
    }

    private async UniTask Awake()
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

        // 임시 아이템 채워넣기
        InventoryData.Instance.InitInventory();

        // 게임 설정이 완료될 때까지 대기
        await InitGame();

        // 게임 시작 전 설정
        StartGame();
    }

    private async UniTask InitGame()
    {
        // 로컬리제이션 로드
        await InitLocalized();

        // 시나리오 로드
        await LoadScenario(GameData.Instance.Chapter);
    }

    private async UniTask InitLocalized()
    {
        // 로컬라이제이션 로드 대기
        await LocalizationSettings.InitializationOperation.Task;
    }

    private async UniTask LoadScenario(Chapter data)
    {
        var scenarioManager = ScenarioManager.Instance;

        int chapter = data.ChapterNum;
        int root = data.RootNum;
        int subChapter = data.SubChapterNum;

        await scenarioManager.LoadScenario(chapter, root, subChapter);
    }

    public void StartGame()
    {

    }

    /// <summary>
    /// 현재 게임 상태(타이틀? 필드? 전투?) 변경에 따른 플레이어 오브젝트 활성화 설정
    /// </summary>
    public void OnGameStateChanged()
    {
        if (GameData.Instance.State == GameState.Battle)
        {
            // 전투 시엔 캐릭터 및 트래킹 카메라 오브젝트 비활성화
            playerObj.SetActive(false);
            trackingCamera.SetActive(false);
        }
        else if (GameData.Instance.State == GameState.Field)
        {
            // 필드로 돌아올 경우 다시 오브젝트 활성화
            playerObj.SetActive(true);
            trackingCamera.SetActive(true);
        }
    }
}