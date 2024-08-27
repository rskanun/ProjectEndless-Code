using System;
using System.Collections.Generic;

public class RunAction : BattleAction
{
    public RunAction()
    {
        actionType = ActionType.Run;
    }

    public override void OnAction()
    {
        actor.OnRun();
    }

    public override void SetTarget(List<Entity> targets)
    {
        // Å¸°Ù ¼³Á¤ X
        throw new NotSupportedException();
    }

    public override TargetType GetTargetType()
    {
        return TargetType.None;
    }
}