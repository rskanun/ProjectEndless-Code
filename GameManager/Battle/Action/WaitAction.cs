public class WaitAction : BattleAction
{
    public Entity target;

    public WaitAction(float turn)
    {
        remainTurn = turn;
        action = ActionType.Wait;
    }

    public override BattleAction Clone()
    {
        WaitAction clone = new WaitAction(remainTurn);

        clone.target = target;

        return clone;
    }

    public override void OnAction()
    {
        target.OnWating();
    }
}