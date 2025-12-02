using System.Linq;
using UnityEngine;

public class Npc : MonoBehaviour
{
    [SerializeField]
    private NpcData npc;
    private ScenarioScene dialogueScene;

    public int GetID()
    {
        return npc.ID;
    }

    public ScenarioScene GetDialogueScene()
    {
        // 대사를 가지고 있지 않는 경우
        if (dialogueScene == null)
        {
            // 해당 npc의 id에 해당하는 대사 찾아 담기
            dialogueScene = ScenarioManager.Instance.GetNpcDialogueScene(npc.ID);
        }

        return dialogueScene;
    }

    public QuestData GetAcceptableQuest()
    {
        return npc.Quests.FirstOrDefault(quest =>
            QuestManager.Instance.IsAcceptableQuest(quest));
    }

    public QuestData GetAcceptedQuest()
    {
        return npc.Quests.FirstOrDefault(quest =>
            QuestManager.Instance.GetQuestState(quest) == QuestState.OnGoing);
    }

    public QuestData GetCompletableQuest()
    {
        return npc.Quests.FirstOrDefault(quest =>
            QuestManager.Instance.IsCompletableQuest(quest));
    }
}
