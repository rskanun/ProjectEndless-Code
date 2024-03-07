public class Friendly : Propensity
{
    public Friendly(Monster monster)
    {
        this.monster = monster;
    }

    public override void OnIdleAction(FSM fsm)
    {
        if (monster is FriendlyMonster)
        {
            // 주변으로 긍정적인 효과 부여
            FriendlyMonster friendlyMonster = (FriendlyMonster)monster;
            friendlyMonster.ProvideEffect();
        }
    }

    public override void OnAttacked(FSM fsm)
    {
        // 적대적 성향으로 바뀜
        monster.CurPropensity = new Hostile();

        base.OnAttacked(fsm);
    }
}