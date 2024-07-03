public class AttackAction : BattleAction
{
    public Entity target;

    public AttackAction()
    {
        action = ActionType.Attack;
    }

    public override BattleAction Clone()
    {
        AttackAction clone = new AttackAction();

        clone.remainTurn = remainTurn;
        clone.actor = actor;
        clone.target = target;

        return clone;
    }

    public override void OnAction()
    {
        actor.OnAttack(target);
    }
}