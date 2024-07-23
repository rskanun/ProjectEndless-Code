public class RunAction : BattleAction
{
    public RunAction()
    {
        actionType = ActionType.Run;
    }

    public override void OnAction()
    {
        actor.OnRun();
    }
}