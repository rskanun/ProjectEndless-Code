using Assets.Script.Control.Text.Object;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField]
    private NPCData npc;

    public int getID()
    {
        return npc.Id;
    }

    public List<Line> getLines()
    {
        return npc.Lines;
    }
}
