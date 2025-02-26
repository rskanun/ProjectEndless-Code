using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NpcData
{
    [SerializeField]
    private int _chrNum;
    [SerializeField]
    private int _idx;
    /***************************************************************
     * [ 고유 번호 ]
     * 
     * 플레이어와의 상호작용에 쓰일 고유 번호
     * 캐릭터 번호 3자리 + 순서번호 3자리로 구성
     ****************************************************************/
    public int ID
    {
        get
        {
            // 아이디 값은 캐릭터 번호 3자리와 순서번호 3자리로 구성
            int id = _chrNum * 1000 + _idx;

            // 해당 npc가 대사를 가지고 있지 않은 경우 임시적으로 0번을 리턴
            if (id != 0 && !TextScriptResource.Instance.HasLines(id))
            {
                return 0;
            }

            return id;
        }
    }

    private List<Line> _lines;
    public List<Line> Lines
    {
        get
        {
            if (_lines != null) return _lines;

            // 해당 npc의 id에 해당하는 대사가 존재할 경우에만 담기
            if (TextScriptResource.Instance.HasLines(ID))
            {
                TextScript script = TextScriptResource.Instance.CurrentScript;

                _lines = script.GetLines(ID);
            }

            return _lines;
        }
    }
}
