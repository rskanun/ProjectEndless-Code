public class Neutral : Propensity
{
    public override void OnIdleAction(FSM fsm) { }

    public override void OnAttacked(FSM fSM)
    {
        throw new System.NotImplementedException();
    }
}