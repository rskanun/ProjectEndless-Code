public class RunAction : BattleAction
{
    public RunAction()
    {
        action = ActionType.Run;
    }

    public override void OnAction()
    {
        actor.OnRun();
    }
}