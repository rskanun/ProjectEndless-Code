public class RunAction : BattleAction
{
    public Entity target;

    public RunAction(float turn)
    {
        remainTurn = turn;
        action = ActionType.Run;
    }

    public override BattleAction Clone()
    {
        RunAction clone = new RunAction(remainTurn);

        clone.target = target;

        return clone;
    }

    public override void OnAction()
    {
        target.OnRun();
    }
}