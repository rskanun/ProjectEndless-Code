public class Friendly : Propensity
{
    private FriendlyMonster friendlyMonster;

    public Friendly(FriendlyMonster monster) : base(monster)
    {
        friendlyMonster = monster;
    }

    public override void OnIdleAction(FSM fsm)
    {
        friendlyMonster.ProvideEffect();
    }

    public override void OnAttacked(FSM fsm)
    {
        // 적대적 성향으로 바뀜
        monster.CurPropensity = new Hostile(monster);

        base.OnAttacked(fsm);
    }
}