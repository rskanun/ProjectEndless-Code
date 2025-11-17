using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Npc : MonoBehaviour
{
    [SerializeField]
    private NpcData npc;

    [SerializeField] // 퀘스트 정보
    private List<QuestData> quests;

    public int GetID()
    {
        return npc.ID;
    }

    public List<Line> GetLines()
    {
        return npc.Lines;
    }

    public QuestData GetAcceptedQuest()
    {
        // 해당 NPC의 진행 중인 퀘스트 리턴
        return quests.FirstOrDefault(quest
            => GameData.Instance.CurrentQuest == quest);
    }

    public QuestData GetAcceptableQuest()
    {
        // 조건을 만족하는 수주 가능한 퀘스트 리턴 
        return quests.FirstOrDefault(quest
            => QuestManager.Instance.IsCompletedQuest(quest) == false   // 완료 되지 않아야 함
                && quest != GameData.Instance.CurrentQuest              // 현재 퀘스트가 아니어야 함
                && (quest.RequiredQuest == null                         // 선행퀘가 없거나 완료되어야 함
                || QuestManager.Instance.IsCompletedQuest(quest.RequiredQuest)));
    }

    public QuestData GetCompletableQuest()
    {
        // 현재 퀘스트 가져오기
        QuestData currentQuest = GameData.Instance.CurrentQuest;

        // 현재 퀘스트가 null이 아니고, 해당 NPC가 목표 대상이면 반환
        return currentQuest != null && currentQuest.ObjectID == GetID() ? currentQuest : null;
    }

    public bool IsInteractive()
    {
        return npc.Lines != null;
    }
}
