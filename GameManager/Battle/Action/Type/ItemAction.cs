using System.Collections.Generic;

public class ItemAction : BattleAction
{
    public Consumable usingItem;
    public List<Entity> targets;

    public ItemAction()
    {
        actionType = ActionType.Item;
    }

    public override void OnAction()
    {
        actor.OnUseItem(usingItem, targets);
    }

    public override void SetTarget(List<Entity> targets)
    {
        this.targets = targets.ConvertAll(entity => entity);
    }

    public override TargetType GetTargetType()
    {
        return usingItem.TargetType;
    }
}