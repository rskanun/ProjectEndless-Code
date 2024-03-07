public class Neutral : Propensity
{
    public Neutral(Monster monster)
    {
        this.monster = monster;
    }

    public override void OnIdleAction(FSM fsm) { }

    public override void OnAttacked(FSM fsm)
    {
        // 적대적 성향으로 바뀜
        monster.CurPropensity = new Hostile();

        base.OnAttacked(fsm);
    }
}