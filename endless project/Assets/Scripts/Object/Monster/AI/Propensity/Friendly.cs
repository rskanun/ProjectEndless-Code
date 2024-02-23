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
        // 플레이어 추적 상태로 변경
        base.OnAttacked(fsm);
    }
}