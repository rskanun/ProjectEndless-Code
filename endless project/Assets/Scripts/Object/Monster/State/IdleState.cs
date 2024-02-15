public class IdleState : IMonsterState
{
    private static IdleState _instance;
    public static IdleState Instance
    {
        get
        {
            if (_instance == null) 
                _instance = new IdleState();

            return _instance;
        }
    }

    public void OnEnterState(AIMonsterControlled monsterAI)
    {
        monsterAI.OnEnterIdle();
    }

    public void OnAction(AIMonsterControlled monsterAI)
    {
        monsterAI.OnIdleAction();
    }

    public void OnTakeDamage(AIMonsterControlled monsterAI)
    {
        monsterAI.OnTakeDamage();
    }
}