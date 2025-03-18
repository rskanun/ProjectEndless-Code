using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BattleFieldData
{
    // 전투 시 등장할 몬스터
    [SerializeField]
    private List<GameObject> _encountMonsters;
    public List<GameObject> EncountMonsters
    {
        get { return _encountMonsters; }
    }

    // 전투 시 발생할 수 있는 이벤트 스크립트
    [SerializeField]
    private BattleEvent _battleEvent;
    public BattleEvent BattleEvent
    {
        get { return _battleEvent; }
    }
}