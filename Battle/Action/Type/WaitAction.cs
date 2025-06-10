using System;
using System.Collections.Generic;

[Serializable]
public class WaitAction : BattleAction
{
    public WaitAction() : base(ActionType.Wait) { }
    public WaitAction(Entity actor, float remainTurn) : base(ActionType.Wait)
    {
        this.actor = actor;
        this.remainTurn = remainTurn;
    }

    public override void OnAction()
    {
        actor.Wait();
    }

    public override List<Entity> GetTargets()
    {
        return null;
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