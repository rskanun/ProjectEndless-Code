public interface IMonsterState
{
    public void OnEnterState(AIMonsterControlled monsterAI);
    public void OnAction(AIMonsterControlled monsterAI);
    public void OnTakeDamage(AIMonsterControlled monsterAI);
}