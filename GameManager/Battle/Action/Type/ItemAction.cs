public class ItemAction : BattleAction
{
    public Item usingItem;
    public Entity target;

    public ItemAction()
    {
        action = ActionType.Item;
    }

    public override BattleAction Clone()
    {
        ItemAction clone = new ItemAction();

        clone.remainTurn = remainTurn;
        clone.usingItem = usingItem;
        clone.actor = actor;
        clone.target = target;

        return clone;
    }

    public override void OnAction()
    {
        actor.OnUseItem(usingItem, target);
    }
}