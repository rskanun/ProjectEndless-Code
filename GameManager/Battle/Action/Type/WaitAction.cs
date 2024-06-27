public class WaitAction : BattleAction
{
    public Entity target;

    public WaitAction()
    {
        action = ActionType.Wait;
    }

    public override BattleAction Clone()
    {
        WaitAction clone = new WaitAction();

        clone.remainTurn = remainTurn;
        clone.target = target;

        return clone;
    }

    public override void OnAction()
    {
        target.OnWaiting();
    }
}