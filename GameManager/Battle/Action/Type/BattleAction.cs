using System;

public enum ActionType
{
    // 전투에서 취할 수 있는 행동 목록
    Wait,   // 대기(턴 넘기기)
    Attack, // 일반 공격
    Skill,  // 스킬
    Item,   // 아이템 사용
    Run     // 도주
}

public abstract class BattleAction : IComparable<BattleAction>
{
    public float remainTurn;
    public ActionType action;

    public int CompareTo(BattleAction seq)
    {
        if (seq.remainTurn < remainTurn) return 1;
        else return -1;
    }

    public abstract BattleAction Clone();

    public virtual void OnAction() { }
}