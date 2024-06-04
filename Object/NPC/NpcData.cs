using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Object/NPC", fileName = "NPC_Data")]
public class NpcData : ScriptableObject
{
    [SerializeField]
    private int _id;
    /***************************************************************
     * [ 고유 번호 ]
     * 
     * 플레이어와의 상호작용에 쓰일 고유 번호
     * 캐릭터 번호 3자리 + 순서번호 3자리로 구성
     ****************************************************************/
    public int Id {
        get
        {
            // 해당 npc가 대사를 가지고 있지 않은 경우 임시적으로 0번을 리턴
            if(_id != 0 && !ScriptResource.Instance.HasLines(_id))
            {
                return 0;
            }

            return _id; 
        }
    }

    private List<Line> _lines;
    public List<Line> Lines
    {
        get
        {
            if (_lines != null) return _lines;

            // 해당 npc의 id에 해당하는 대사가 존재할 경우에만 담기
            if (ScriptResource.Instance.HasLines(_id))
            {
                Script script = ScriptResource.Instance.CurrentScript;

                _lines = script.GetLines(_id);
            }

            return _lines;
        }
    }
}
