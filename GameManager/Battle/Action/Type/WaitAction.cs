[System.Serializable]
public class WaitAction : BattleAction
{
    public WaitAction()
    {
        actionType = ActionType.Wait;
    }

    public override void OnAction()
    {
        // Nothing
    }
}