public class RunAction : BattleAction
{
    public Entity target;

    public RunAction()
    {
        action = ActionType.Run;
    }

    public override BattleAction Clone()
    {
        RunAction clone = new RunAction();

        clone.remainTurn = remainTurn;
        clone.target = target;

        return clone;
    }

    public override void OnAction()
    {
        target.OnRun();
    }
}