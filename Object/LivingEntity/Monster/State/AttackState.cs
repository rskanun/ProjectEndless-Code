using UnityEngine;

public class AttackState : IMonsterState
{
    private MonsterObject monster;

    private float attackCooldown;

    public AttackState(MonsterObject monster)
    {
        this.monster = monster;
    }

    public void OnEnterState()
    {
        // 쿨타임 초기화
        attackCooldown = 0;
    }

    public void OnAction(FSM fsm)
    {
        if (attackCooldown <= 0)
        {
            // 공격 액션
            OnAttack();

            // 공격 가능한 범위인지 체크
            if (IsAttackable() == false)
            {
                fsm.SetState(new ChaseState(monster));
            }
        }
        else
        {
            attackCooldown -= Time.deltaTime;
        }
    }

    private void OnAttack()
    {
        // 몬스터 각자의 공격 액션
        monster.OnAttack();

        // 공격 후 다음 공격까지 쿨타임 적용
        attackCooldown = monster.AttackCooldown;
    }

    private bool IsAttackable()
    {
        Vector2 playerPos = ReadOnlyPlayerData.Instance.Position;
        float distance = (playerPos - (Vector2)monster.transform.position).magnitude;

        return distance <= monster.AttackDistance;
    }

    public void OnTakeDamage(FSM fsm) { }
}