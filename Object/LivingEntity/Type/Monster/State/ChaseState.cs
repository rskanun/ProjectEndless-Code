using UnityEngine;

public class ChaseState : IMonsterState
{
    private MonsterObject monster;

    private Vector3 lastPlayerPos;

    public ChaseState(MonsterObject monster)
    {
        this.monster = monster;
    }

    public void OnEnterState()
    {
        lastPlayerPos = ReadOnlyPlayerData.Instance.Position;
    }

    public void OnAction(FSM fsm)
    {
        // 플레이어 추적
        ChasePlayer(fsm);
    }

    private void ChasePlayer(FSM fsm)
    {
        Vector3 playerPos = GetPlayerPos();
        if (playerPos != monster.transform.position)
        {
            float distance = ((Vector2)(playerPos - monster.transform.position)).magnitude;
            float attackDistance = monster.AttackDistance;
            
            if (distance <= attackDistance)
            {
                // 공격 범위 안에 있으면 공격
                fsm.SetState(new AttackState(monster));
            }
            else
            {
                // 공격 범위 밖이면 플레이어 추적
                monster.MoveTo(playerPos);
            }
        }
        else
        {
            // 플레이어가 범위 밖으로 벗어나면 탐지 상태로 전환
            fsm.SetState(new IdleState(monster));
        }
    }

    private Vector3 GetPlayerPos()
    {
        // 플레이어 탐지
        Vector3 playerPos = monster.DetectPlayer();

        if (playerPos != monster.transform.position)
        {
            // 플레이어가 탐지 범위 안이면 리턴
            lastPlayerPos = playerPos;
            return playerPos;
        }
        
        return lastPlayerPos;
    }

    public void OnTakeDamage(FSM fsm) { }
}