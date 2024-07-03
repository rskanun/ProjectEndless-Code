using System.Collections.Generic;

public class BattleSequence
{
    // 현재 전투 순서
    private List<BattleAction> _sequence;
    public List<BattleAction> Sequence
    {
        private set { _sequence = value; }
        get { return _sequence; }
    }

    public void SetSequence(List<Entity> entityList)
    {
        Sequence = new List<BattleAction>();

        // 엔티티의 민첩 수치로 내림차순 정렬
        List<Entity> sortedList = new List<Entity>(entityList);
        sortedList.Sort((x, y) => y.Stat.AGI.CompareTo(x.Stat.AGI));

        // 모든 엔티티들은 0턴 대기 행동 시전
        foreach (Entity entity in sortedList)
        {
            WaitAction turnData = new WaitAction();

            turnData.remainTurn = 0.0f;
            turnData.actor = entity;

            Sequence.Add(turnData);
        }
    }

    public void NextTurn()
    {
        float passedTurn = Sequence[0].remainTurn;
        Sequence.RemoveAt(0);

        // 삭제된 턴만큼 수치 앞당기기
        foreach (BattleAction turnData in Sequence)
        {
            turnData.remainTurn -= passedTurn;
        }
    }

    public BattleAction GetCurrentTurn()
    {
        if (Sequence.Count <= 0) return null;
        return Sequence[0];
    }

    public void AddTurn(BattleAction action)
    {
        int index = Sequence.BinarySearch(action);

        if (index < 0) Sequence.Insert(~index, action);
        else Sequence.Insert(index, action);
    }
}