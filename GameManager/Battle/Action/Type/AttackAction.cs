public class AttackAction : BattleAction
{
    public Entity attacker;
    public Entity target;

    public AttackAction()
    {
        action = ActionType.Attack;
    }

    public override BattleAction Clone()
    {
        AttackAction clone = new AttackAction();

        clone.remainTurn = remainTurn;
        clone.attacker = attacker;
        clone.target = target;

        return clone;
    }

    public override void OnAction()
    {
        attacker.OnAttack(target);
    }
}