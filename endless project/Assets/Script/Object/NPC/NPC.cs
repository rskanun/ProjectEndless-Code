using System.Collections;
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
}
