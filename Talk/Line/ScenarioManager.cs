using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine.AddressableAssets;
using Cysharp.Threading.Tasks;
using UnityEngine.Localization.Tables;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Localization.Settings;
using System.IO;
using UnityEditor.AddressableAssets;


#if UNITY_EDITOR
using UnityEditor;
#endif

public class ScenarioManager : ScriptableObject
{
    private const string FILE_DIRECTORY = "Assets/Resources";
    private const string FILE_PATH = "Assets/Resources/ScenarioManager.asset";

    private static ScenarioManager _instance;
    public static ScenarioManager Instance
    {
        get
        {
            if (_instance != null) return _instance;

            _instance = Resources.Load<ScenarioManager>("ScenarioManager");

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
                _instance = AssetDatabase.LoadAssetAtPath<ScenarioManager>(FILE_PATH);

                if (_instance == null)
                {
                    _instance = CreateInstance<ScenarioManager>();
                    AssetDatabase.CreateAsset(_instance, FILE_PATH);
                }
            }
#endif

            return _instance;
        }
    }

    [SerializeField]
    [FolderPath(RequireExistingPath = true)]
    private string scenarioDirectory;

    [Title("Addressable Settings")]
    [SerializeField] private string addressableGroupName;
    [SerializeField] private string labelPrefix;

    // 현재 읽혀지는 시나리오
    private Dictionary<int, Line> scenarioTable = new();

    // 현재 시나리오에 쓰이는 로컬라이제이션 테이블
    private List<StringTable> nameTableList = new();
    private List<StringTable> dialogueTableList = new();
    private List<StringTable> selectionTableList = new();

    // 언어 변경에 따른 테이블 리로드를 위한 테이블 목록
    private HashSet<string> requireNameTables = new();
    private HashSet<string> requireDialogueTables = new();
    private HashSet<string> requireSelectionTables = new();

    // 메모리 해제를 위한 로드 핸들 리스트
    private List<AsyncOperationHandle> loadHandles = new();

#if UNITY_EDITOR
    [Button(ButtonSizes.Large, Name = "Update Addressable Group")]
    public void UpdateAddressableGroup()
    {
        // 경로상 폴더가 존재하지 않는 경우
        if (string.IsNullOrEmpty(scenarioDirectory) || !Directory.Exists(scenarioDirectory))
        {
            // 실행하지 않고 종료
            return;
        }

        // Addressable 셋팅과 그룹 찾아오기
        var settings = AddressableAssetSettingsDefaultObject.Settings;
        var group = settings.FindGroup(addressableGroupName);

        // 그룹이 없는 경우
        if (group == null)
        {
            // 새로 만들기
            group = settings.CreateGroup(addressableGroupName, false, false, true, null);
        }

        // 시나리오 폴더 탐색
        foreach (var path in Directory.GetDirectories(scenarioDirectory))
        {
            var folderName = Path.GetFileName(path);
            var label = $"{labelPrefix}_{folderName}";

            // 폴더 내 시나리오 에셋 탐색
            var guids = AssetDatabase.FindAssets("t:Scenario", new string[] { path });
            foreach (var guid in guids)
            {
                // 에셋을 그룹에 등록
                var entry = settings.CreateOrMoveEntry(guid, group);

                if (entry == null) continue;

                // 그룹 내 레이블 등록
                settings.AddLabel(label);
                entry.SetLabel(label, true, true);
            }
        }

        // 변경사항 저장
        EditorUtility.SetDirty(settings);
    }
#endif

    public string GetLocalizedName(string key)
    {
        return GetLocalizedValue(nameTableList, key);
    }

    public string GetLocalizedDialogue(string key)
    {
        return GetLocalizedValue(dialogueTableList, key);
    }

    public string GetLocalizedSelection(string key)
    {
        return GetLocalizedValue(selectionTableList, key);
    }

    private string GetLocalizedValue(List<StringTable> tableList, string key)
    {
        // 테이블 탐색
        foreach (var table in tableList)
        {
            var entry = table.GetEntry(key);

            // 해당 테이블에 등록된 key 값인 경우 리턴
            if (entry != null)
                return entry.Value;
        }

        return null;
    }

    public async UniTask LoadScenario(int chapter, int root, int subChapter)
    {
        // 현재 시나리오 초기화
        scenarioTable.Clear();

        // Addressable에서 찾아올 레이블 이름
        string labelName = $"{labelPrefix}_{chapter}{root}{subChapter:d2}";

        // 유효한지 먼저 판단
        if (!await IsValidLabel(labelName))
        {
            // 유효하지 않다면 시나리오 로드 즉시 종료
            Debug.LogError($"유효하지 않은 레이블입니다. 문제가 발생한 레이블: {labelName}");
            return;
        }

        // 현재 진행 상황에 맞는 Scenario 에셋 비동기로 로드
        var handle = Addressables.LoadAssetsAsync<Scenario>(labelName, null);
        IList<Scenario> scenarios = await handle.Task;

        foreach (var scenario in scenarios)
        {
            foreach (var id in scenario.IDs)
            {
                var intro = scenario.GetIntroLine(id);

                // 읽기 전용 리스트를 일반 리스트로 변환해서 넣어주기
                scenarioTable.Add(id, intro);
            }
        }

        // 해당 시나리오에 쓰이는 로컬라이제이션 테이블 비동기로 로드
        await LoadLocalizationTables(scenarios);

        // 시나리오 정보만을 가져온 후 해제
        Addressables.Release(handle);
    }

    private async UniTask<bool> IsValidLabel(string labelName)
    {
        // 미리 해당 레이블을 가진 에셋이 있는 지 주소 값을 불러오기
        var handle = Addressables.LoadResourceLocationsAsync(labelName);

        try
        {
            var locations = await handle.Task;

            // 존재 여부로 유효한 레이블인지 판단
            return handle.Status == AsyncOperationStatus.Succeeded &&
                    locations != null &&
                    locations.Count > 0;
        }
        catch
        {
            // 만약 로드 중에 오류가 있다면 유효하지 않다고 판단
            return false;
        }
        finally
        {
            // 핸들은 어떠한 상황에서도 무조건 해제
            Addressables.Release(handle);
        }
    }

    private async UniTask LoadLocalizationTables(IEnumerable<Scenario> scenarios)
    {
        // 필요한 테이블 목록 설정
        requireNameTables.Clear();
        requireDialogueTables.Clear();
        requireSelectionTables.Clear();

        foreach (var scenario in scenarios)
        {
            requireNameTables.Add(scenario.nameTable);
            requireDialogueTables.Add(scenario.dialogueTable);
            requireSelectionTables.Add(scenario.selectionTable);
        }

        // 테이블 목록 비동기로 불러오기
        await ReloadLocalizationTables();
    }

    public async UniTask ReloadLocalizationTables()
    {
        // 기존 핸들 메모리 해제
        foreach (var handle in loadHandles)
        {
            // 유효하지 않은 핸들은 넘기기
            if (!handle.IsValid()) continue;

            Addressables.Release(handle);
        }

        // 이전 테이블 목록 초기화
        loadHandles.Clear();
        nameTableList.Clear();
        dialogueTableList.Clear();
        selectionTableList.Clear();

        // 테이블 다시 불러오기
        foreach (var tableName in requireNameTables)
        {
            var table = await LoadStringTable(tableName);
            nameTableList.Add(table);
        }
        foreach (var tableName in requireDialogueTables)
        {
            var table = await LoadStringTable(tableName);
            dialogueTableList.Add(table);
        }
        foreach (var tableName in requireSelectionTables)
        {
            var table = await LoadStringTable(tableName);
            selectionTableList.Add(table);
        }
    }

    private async UniTask<StringTable> LoadStringTable(string tableName)
    {
        // 테이블을 비동기로 불러오기
        var handler = LocalizationSettings.StringDatabase.GetTableAsync(tableName);
        await handler.Task;

        // 불러오는데 성공한 경우
        if (handler.Status == AsyncOperationStatus.Succeeded)
        {
            // 이후 메모리 해제를 위한 리스트에 추가
            loadHandles.Add(handler);

            // 결과값 리턴
            return handler.Result;
        }

        // 불러오는데 실패했다면 경고문 띄우기
        Debug.LogError($"{tableName}을 불러오는데 실패했습니다");
        return null;
    }

    public Line GetNpcDialogueIntro(int npcID)
    {
        int scenarioNum = GetScenarioNumByNpc(npcID);

        return GetIntroLine(scenarioNum);
    }

    public Line GetQuestIntro(int questID, QuestState state)
    {
        int scenarioNum = GetScenarioNumByQuest(questID, (int)state);

        return GetIntroLine(scenarioNum);
    }

    public int GetScenarioNumByNpc(int npcID)
    {
        // NPC의 일반적인 대사의 경우
        // NPC 판별 번호 1 + NPC 아이디 6자리를 합쳐 시나리오 번호로 지정
        // ex) NPC 판별 번호 1 + NPC 아이디 001000 => 시나리오 번호 1001000
        return int.Parse("1" + npcID.ToString("D6"));
    }

    public int GetScenarioNumByQuest(int questID, int stateNum)
    {
        // 퀘스트에 따른 NPC의 대사의 경우
        // 퀘스트 판별 번호 2 + 퀘스트 아이디 5자리 + 상태 번호 1자리를 합쳐 시나리오 번호로 지정
        // ex) 퀘스트 판별 번호 2 + 퀘스트 아이디 1 + 상태 번호 1 => 시나리오 번호 2000011
        return int.Parse("2" + questID.ToString("D5") + stateNum);
    }

    private Line GetIntroLine(int id)
    {
        return scenarioTable[id];
    }
}