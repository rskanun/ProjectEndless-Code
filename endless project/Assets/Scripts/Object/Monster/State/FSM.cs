public class FSM
{
    private IMonsterState _currentState;

    public FSM(IMonsterState state)
    {
        _currentState = state;
    }

    public void SetState(IMonsterState state)
    {
        _currentState = state;
    }

    public void UpdateState()
    {
        if (_currentState != null)
        {
            // _currentState.OnAction();
        }
    }
}