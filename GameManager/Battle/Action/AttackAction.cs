public class AttackAction : BattleAction
{
    public Entity attacker;
    public Entity target;

    public AttackAction(float turn)
    {
        remainTurn = turn;
        action = ActionType.Attack;
    }

    public override BattleAction Clone()
    {
        AttackAction clone = new AttackAction(remainTurn);

        clone.attacker = attacker;
        clone.target = target;

        return clone;
    }

    public override void OnAction()
    {
        attacker.OnAttack(target);
    }
}