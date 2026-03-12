using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NpcData
{
    [SerializeField]
    private int _id;
    public int ID => _id;

    [SerializeField]
    private List<QuestData> _quests;
    public List<QuestData> Quests => _quests;
}
