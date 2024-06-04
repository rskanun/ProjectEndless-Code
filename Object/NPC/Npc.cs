using System.Collections.Generic;
using UnityEngine;

public class Npc : MonoBehaviour
{
    [SerializeField]
    private NpcData npc;

    public int getID()
    {
        return npc.Id;
    }

    public List<Line> getLines()
    {
        return npc.Lines;
    }

    public bool isInteractive()
    {
        return npc.Lines != null;
    }
}
