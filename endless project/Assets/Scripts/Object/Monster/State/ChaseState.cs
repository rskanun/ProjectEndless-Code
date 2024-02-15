public class ChaseState : IMonsterState
{
    private static ChaseState _instance;
    public static ChaseState Instance
    {
        get
        {
            if (_instance == null)
                _instance = new ChaseState();

            return _instance;
        }
    }

    public void OnEnterState(AIMonsterControlled monsterAI)
    {

    }

    public void OnAction(AIMonsterControlled monsterAI)
    {
        
    }

    public void OnTakeDamage(AIMonsterControlled monsterAI)
    {

    }
}