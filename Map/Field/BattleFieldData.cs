using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BattleFieldData
{
    // 현재 필드 내 있는 몬스터 정보
    [SerializeField]
    private List<GameObject> _fieldMonsters;
    public List<GameObject> FieldMonsters
    {
        get { return _fieldMonsters; }
    }

    // 전투 시 발생할 수 있는 이벤트 스크립트
    [SerializeField]
    private BattleEvent _battleEvent;
    public BattleEvent BattleEvent
    {
        get { return _battleEvent; }
    }
}