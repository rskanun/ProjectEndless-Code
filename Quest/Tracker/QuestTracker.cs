using TMPro;
using UnityEngine;

public class QuestTracker : MonoBehaviour
{
    [SerializeField] private GameObject tracker;
    [SerializeField] private TextMeshProUGUI content;

    private void OnEnable()
    {
        QuestManager.Instance.onTrackedQuestChanged += OnQuestUpdate;
    }

    private void OnDisable()
    {
        QuestManager.Instance.onTrackedQuestChanged -= OnQuestUpdate;
    }

    private void Start()
    {
        OnQuestUpdate(QuestManager.Instance.TrackedQuest);
    }

    private void OnQuestUpdate(QuestData quest)
    {
        // 퀘스트가 있으면 알림 키고 업데이트, 없으면 숨기기
        if (quest != null) ShowTracker(quest);
        else HideTracker();
    }

    private void ShowTracker(QuestData quest)
    {
        tracker.SetActive(true);
        content.text = quest.Content;
    }

    private void HideTracker()
    {
        tracker.SetActive(false);
    }
}