using System;
using System.Collections.Generic;

[System.Serializable]
public class TurnData : IComparable<TurnData>
{
    public float remainTurn;
    public Entity entity;

    public int CompareTo(TurnData seq)
    {
        if (seq.remainTurn < remainTurn) return 1;
        else return -1;
    }
}

public class BattleSeq
{
    public List<TurnData> sequence;

    public BattleSeq()
    {
        sequence = new List<TurnData>();
    }

    public BattleSeq(List<TurnData> seq)
    {
        sequence = seq;
    }

    public BattleSeq(List<Entity> entityList)
    {
        // 엔티티의 민첩 수치로 내림차순 정렬
        List<Entity> sortedList = new List<Entity>(entityList);
        sortedList.Sort((x, y) => y.Stat.AGI.CompareTo(x.Stat.AGI));

        // 모든 엔티티 값들을 TurnData로 전환
        foreach (Entity entity in sortedList)
        {
            TurnData turnData = new TurnData();

            turnData.remainTurn = 0;
            turnData.entity = entity;

            sequence.Add(turnData);
        }
    }

    public void SumSequence(BattleSeq seq)
    {
        sequence.AddRange(seq.sequence);
    }

    public TurnData GetCurrentTurn()
    {
        if (sequence.Count <= 0) return null;
        return sequence[0];
    }


}