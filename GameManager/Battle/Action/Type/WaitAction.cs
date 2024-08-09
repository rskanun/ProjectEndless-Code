using System.Collections.Generic;
using System;

[System.Serializable]
public class WaitAction : BattleAction
{
    public WaitAction()
    {
        actionType = ActionType.Wait;
    }

    public override void OnAction()
    {
        // Nothing
    }

    public override void SetTarget(List<Entity> targets)
    {
        // Å¸°Ù ¼³Á¤ X
        throw new NotSupportedException();
    }

    public override TargetType GetTargetType()
    {
        return TargetType.Caster;
    }
}