public class AttackAction : BattleAction
{
    public Entity target;

    public AttackAction()
    {
        action = ActionType.Attack;
    }

    public override void OnAction()
    {
        actor.OnAttack(target);
    }
}