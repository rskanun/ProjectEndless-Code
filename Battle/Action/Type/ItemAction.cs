using System;
using System.Collections.Generic;

[Serializable]
public class ItemAction : BattleAction
{
    public Consumable usingItem;
    public List<Entity> targets;

    public ItemAction() : base(ActionType.Item) { }
    public ItemAction(Entity actor, Consumable usingItem, List<Entity> targets, float remainTurn) : base(ActionType.Item)
    {
        this.actor = actor;
        this.usingItem = usingItem;
        this.targets = targets;
        this.remainTurn = remainTurn;
    }

    public override void OnAction()
    {
        actor.OnUseItem(usingItem, targets);
    }

    public override List<Entity> GetTargets()
    {
        return targets;
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