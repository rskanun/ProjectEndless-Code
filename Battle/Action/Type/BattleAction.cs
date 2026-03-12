using System;
using System.Collections.Generic;

public enum ActionType
{
    // 전투에서 취할 수 있는 행동 목록
    Wait,   // 첫 턴 진행
    Attack, // 일반 공격
    Skill,  // 스킬
    Item,   // 아이템 사용
    Run     // 도주
}

[Serializable]
public abstract class BattleAction : IComparable<BattleAction>
{
    public Entity actor;
    public float remainTurn;

    private ActionType _actionType;
    public ActionType ActionType
    {
        private set { _actionType = value; }
        get { return _actionType; }
    }

    public BattleAction(ActionType type)
    {
        ActionType = type;
    }

    public int CompareTo(BattleAction seq)
    {
        if (seq.remainTurn < remainTurn) return 1;
        else return -1;
    }

    public abstract void OnAction();

    public abstract List<Entity> GetTargets();

    public abstract void SetTarget(List<Entity> targets);

    public abstract TargetType GetTargetType();
}