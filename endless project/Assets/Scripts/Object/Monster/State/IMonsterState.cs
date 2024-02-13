public interface IMonsterState
{
    public void OnAction(AIMonsterControlled monsterAI);
    public void OnTakeDamage(AIMonsterControlled monsterAI);
}