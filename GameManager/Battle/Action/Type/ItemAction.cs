public class ItemAction : BattleAction
{
    public Consumable usingItem;
    public Entity target;

    public ItemAction()
    {
        action = ActionType.Item;
    }

    public override void OnAction()
    {
        actor.OnUseItem(usingItem, target);
    }
}