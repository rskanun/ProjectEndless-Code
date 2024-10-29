using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField]
    private NpcData npc;

    public int GetID()
    {
        return npc.Id;
    }

    public List<Line> GetLines()
    {
        return npc.Lines;
    }

    public bool IsInteractive()
    {
        return npc.Lines != null;
    }
}
