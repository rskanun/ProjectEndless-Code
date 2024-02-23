public abstract class Propensity
{
    protected Monster monster;

    public abstract void OnIdleAction(FSM fsm);
    public virtual void OnAttacked(FSM fsm)
    {
        // 공격당할 시 추적상태로 변경
        fsm.SetState(new ChaseState(monster));
    }
}