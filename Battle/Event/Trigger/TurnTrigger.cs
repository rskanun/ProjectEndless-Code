public class TurnTrigger : EventTrigger
{
    public float turn;

    public override bool IsTrigger()
    {
        return turn <= BattleData.Instance.PassedTurn;
    }
}