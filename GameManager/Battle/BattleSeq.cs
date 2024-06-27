using System.Collections.Generic;

public class BattleSeq
{
    public List<BattleAction> sequence;

    public BattleSeq()
    {
        sequence = new List<BattleAction>();
    }

    public BattleSeq(List<Entity> entityList)
    {
        sequence = new List<BattleAction>();

        // 엔티티의 민첩 수치로 내림차순 정렬
        List<Entity> sortedList = new List<Entity>(entityList);
        sortedList.Sort((x, y) => y.Stat.AGI.CompareTo(x.Stat.AGI));

        // 모든 엔티티들은 0턴 대기 행동 시전
        foreach (Entity entity in sortedList)
        {
            WaitAction turnData = new WaitAction();

            turnData.remainTurn = 0.0f;
            turnData.target = entity;

            sequence.Add(turnData);
        }
    }

    public void SumSequence(BattleSeq seq)
    {
        sequence.AddRange(seq.sequence);
    }

    public void NextTurn()
    {
        float passedTurn = sequence[0].remainTurn;
        sequence.RemoveAt(0);

        // 삭제된 턴만큼 수치 앞당기기
        foreach (BattleAction turnData in sequence)
        {
            turnData.remainTurn -= passedTurn;
        }
    }

    public BattleAction GetCurrentTurn()
    {
        if (sequence.Count <= 0) return null;
        return sequence[0];
    }

    public void SetTurn(BattleAction action)
    {
        int index = sequence.BinarySearch(action);

        if (index < 0) sequence.Insert(~index, action);
        else sequence.Insert(index, action);
    }
}