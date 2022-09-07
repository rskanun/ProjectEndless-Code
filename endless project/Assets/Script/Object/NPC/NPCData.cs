using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "NPC", menuName = "scriptable Object/NPC", order = 1)]
public class NPCData : ScriptableObject
{
    [SerializeField]
    private int id; // 상호작용에 쓰일 고유 번호
    public int Id { get { return id; } }
}
