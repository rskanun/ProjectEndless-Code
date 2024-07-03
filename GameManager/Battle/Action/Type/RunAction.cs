public class RunAction : BattleAction
{
    public RunAction()
    {
        action = ActionType.Run;
    }

    public override BattleAction Clone()
    {
        RunAction clone = new RunAction();

        clone.remainTurn = remainTurn;
        clone.actor = actor;

        return clone;
    }

    public override void OnAction()
    {
        actor.OnRun();
    }
}