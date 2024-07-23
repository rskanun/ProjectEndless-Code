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
}