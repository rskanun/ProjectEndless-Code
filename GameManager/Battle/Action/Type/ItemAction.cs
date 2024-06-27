public class ItemAction : BattleAction
{
    public Item usingItem;
    public Entity user;
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
        clone.user = user;
        clone.target = target;

        return clone;
    }

    public override void OnAction()
    {
        user.OnUseItem(usingItem, target);
    }
}