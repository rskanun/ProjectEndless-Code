public class ItemAction : BattleAction
{
    public Item usingItem;
    public Entity target;

    public ItemAction(float turn)
    {
        remainTurn = turn;
        action = ActionType.Item;
    }

    public override BattleAction Clone()
    {
        ItemAction clone = new ItemAction(remainTurn);

        clone.usingItem = usingItem;
        clone.target = target;

        return clone;
    }

    public override void OnAction()
    {
        usingItem.OnUse(target);
    }
}