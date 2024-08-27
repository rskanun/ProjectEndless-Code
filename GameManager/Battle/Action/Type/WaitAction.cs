using System.Collections.Generic;

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
    }

    public override TargetType GetTargetType()
    {
        return TargetType.None;
    }
}