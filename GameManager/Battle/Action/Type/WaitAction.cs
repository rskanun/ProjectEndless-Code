[System.Serializable]
public class WaitAction : BattleAction
{
    public WaitAction()
    {
        action = ActionType.Wait;
    }

    public override void OnAction()
    {
        // Nothing
    }
}