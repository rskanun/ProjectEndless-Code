using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    private List<BattleSeq> battleSequence = new List<BattleSeq>();

    public void OnBattle(FieldMobData fieldMobData)
    {

    }

    [Serializable]
    private class BattleSeq : IComparable<BattleSeq>
    {
        public float sequence;
        public Entity entity;

        public int CompareTo(BattleSeq seq)
        {
            if (seq.sequence < sequence) return 1;
            else return -1;
        }
    }
}