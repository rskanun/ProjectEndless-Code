public class AttackAction : BattleAction
{
    public Entity target;

    public AttackAction()
    {
        actionType = ActionType.Attack;
    }

    public override void OnAction()
    {
        actor.OnAttack(target);
    }
}