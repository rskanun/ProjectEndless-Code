using System;
using System.Collections.Generic;

[Serializable]
public class RunAction : BattleAction
{
    public RunAction() : base(ActionType.Run) { }
    public RunAction(Entity actor, float remainTurn) : base(ActionType.Run)
    {
        this.actor = actor;
        this.remainTurn = remainTurn;
    }

    public override void OnAction()
    {
        actor.Run();
    }

    public override List<Entity> GetTargets()
    {
        return null;
    }

    public override void SetTarget(List<Entity> targets)
    {
        // 타겟 설정 X
        throw new NotSupportedException();
    }

    public override TargetType GetTargetType()
    {
        return TargetType.None;
    }
}