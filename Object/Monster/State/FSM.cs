public class FSM
{
    // 현재 몬스터 상태
    private IMonsterState _curState;

    public FSM(IMonsterState initState)
    {
        SetState(initState);
    }

    public void SetState(IMonsterState state)
    {
        _curState = state;
        _curState.OnEnterState();
    }

    public void OnAction()
    {
        _curState.OnAction(this);
    }

    public void OnTakeDamage()
    {
        _curState.OnTakeDamage(this);
    }
}