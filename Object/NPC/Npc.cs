using System.Collections.Generic;
using UnityEngine;

public class Npc : MonoBehaviour
{
    [SerializeField]
    private NpcData npc;

    public int GetID()
    {
        return npc.ID;
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
