using UnityEngine;

public class AttackState : IMonsterState
{
    private MonsterObject monster;

    public AttackState(MonsterObject monster)
    {
        this.monster = monster;
    }

    public void OnAction(FSM fsm)
    {
        if (monster.IsAttacked) return;

        // 공격 가능한 범위인지 체크
        if (IsAttackable())
        {
            // 방향 전환
            monster.RotateTo(ReadOnlyGameData.Instance.Position);

            // 공격 액션
            monster.OnAttack();
        }
        else
        {
            // 공격 범위 밖으로 멀어지면 다시 추격
            fsm.SetState(new ChaseState(monster));
        }
    }

    private bool IsAttackable()
    {
        Vector2 playerPos = ReadOnlyGameData.Instance.Position;
        float distance = Vector2.Distance(playerPos, monster.transform.position);

        return distance <= monster.AttackDistance;
    }

    public void OnEnterState() { }
    public void OnTakeDamage(FSM fsm) { }
}