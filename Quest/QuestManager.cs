using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Linq;




#if UNITY_EDITOR
using UnityEditor;
#endif

public class QuestManager : ScriptableObject
{
    // 저장 파일 위치
    private const string FILE_DIRECTORY = "Assets/Resources/Quests";
    private const string FILE_PATH = "Assets/Resources/Quests/QuestManager.asset";

    private static QuestManager _instance;
    public static QuestManager Instance
    {
        get
        {
            if (_instance != null) return _instance;

            _instance = Resources.Load<QuestManager>("Quests/QuestManager");

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
                _instance = AssetDatabase.LoadAssetAtPath<QuestManager>(FILE_PATH);
                if (_instance == null)
                {
                    _instance = CreateInstance<QuestManager>();
                    AssetDatabase.CreateAsset(_instance, FILE_PATH);
                }
            }
#endif
            return _instance;
        }
    }

    [SerializeField]
    [FolderPath(RequireExistingPath = true)]
    private string filePath;

    [ReadOnly, SerializeField]
    private List<QuestData> questDatas = new();

    // 퀘스트 상태 정리 테이블 => 퀘스트 ID, 퀘스트 상태
    private Dictionary<int, QuestState> stateLookup = new();

#if UNITY_EDITOR
    [Button(ButtonSizes.Large, Name = "Reload Quest Files")]
    public void LoadQuestAssets()
    {
        // 경로상 폴더가 존재하지 않는 경우
        if (string.IsNullOrEmpty(filePath) || !AssetDatabase.IsValidFolder(filePath))
        {
            // 실행하지 않고 종료
            Debug.LogError("경로 상에 폴더가 존재하지 않습니다. 폴더 경로: " + filePath);
            return;
        }

        string[] guids = AssetDatabase.FindAssets("t:QuestData", new[] { filePath });
        questDatas = guids.Select(guid => AssetDatabase.LoadAssetAtPath<QuestData>(AssetDatabase.GUIDToAssetPath(guid)))
            .Where(asset => asset != null)
            .ToList();
    }
#endif

    public QuestData FindQuest(int id)
    {
        return questDatas.FirstOrDefault(quest => quest.ID == id);
    }

    public QuestState GetQuestState(QuestData quest)
    {
        if (stateLookup.TryGetValue(quest.ID, out QuestState state))
        {
            return state;
        }

        // 상태가 저장되지 않은 퀘스트인 경우 아직 안 받은 퀘스트로 처리
        return QuestState.Inactive;
    }

    public bool IsAcceptableQuest(QuestData quest)
    {
        // 퀘스트 수주 가능 조건 판단
        // 현재는 진행 중이거나 완료되지 않은 퀘스트면 가능
        return !stateLookup.ContainsKey(quest.ID);
    }

    public bool IsCompletableQuest(QuestData quest)
    {
        // 퀘스트 완료 조건 판단
        // 현재는 진행 중인 퀘스트면 가능
        return GetQuestState(quest) == QuestState.OnGoing;
    }

    public void SetState(QuestData quest, QuestState state)
    {
        // 퀘스트 상태 갱신
        stateLookup[quest.ID] = state;
    }
}