using Assets.Script.Control.Text.Object;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPC", menuName = "scriptable Object/NPC", order = 1)]
public class NPCData : ScriptableObject
{
    [SerializeField]
    private int id; // 상호작용에 쓰일 고유 번호
    public int Id {
        get
        {
            // 해당 npc가 대사를 가지고 있지 않은 경우 임시적으로 0번을 리턴
            if(id != 0 && !CSVReader.Instance.LineData.ContainsKey(id))
            {
                return 0;
            }

            return id; 
        }
    }

    private List<Line> lines;
    public List<Line> Lines
    {
        get
        {
            // 해당 npc의 id에 해당하는 대사가 존재할 경우에만 담기
            if(CSVReader.Instance.LineData.ContainsKey(id))
            {
                if (lines != null) return lines;

                lines = CSVReader.Instance.LineData[id];
            }

            return lines;
        }
    }

}
