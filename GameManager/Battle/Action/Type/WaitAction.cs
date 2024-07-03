public class WaitAction : BattleAction
{
    public WaitAction()
    {
        action = ActionType.Wait;
    }

    public override BattleAction Clone()
    {
        WaitAction clone = new WaitAction();

        clone.remainTurn = remainTurn;
        clone.actor = actor;

        return clone;
    }

    public override void OnAction()
    {
        actor.OnWaiting();
    }
}