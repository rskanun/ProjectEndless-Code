using UnityEngine;

public class QuestManager
{
    public static QuestData FindQuest(int id)
    {
        QuestData[] questDataArray = Resources.LoadAll<QuestData>("Quest");

        foreach (QuestData questData in questDataArray)
        {
            if (questData.ID.Equals(id))
            {
                return questData;
            }
        }

        return null;
    }
}