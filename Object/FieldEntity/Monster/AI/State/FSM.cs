using UnityEngine;

public class FSM
{
    // 현재 몬스터 상태
    private IMonsterState curState;

    public void SetState(IMonsterState state)
    {
        curState = state;
        curState.OnEnterState();
    }

    public void OnAction()
    {
        curState?.OnAction(this);
    }

    public void OnTakeDamage()
    {
        curState?.OnTakeDamage(this);
    }
}