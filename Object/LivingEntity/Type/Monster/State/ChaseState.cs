using UnityEngine;

public class ChaseState : IMonsterState
{
    private MonsterObject monster;

    private Vector3 lastPlayerPos;

    public ChaseState(MonsterObject monster)
    {
        this.monster = monster;
    }

    public void OnAction(FSM fsm)
    {
        // 플레이어 좌표 갱신
        UpdatePlayerPos();

        // 플레이어 추적
        ChasePlayer(fsm);
    }

    private void ChasePlayer(FSM fsm)
    {
        bool isMiss = (Vector2)lastPlayerPos != ReadOnlyGameData.Instance.Position;
        bool isArrive = monster.transform.position == lastPlayerPos;

        // 공격 가능한 거리인지 계산
        float distance = Vector2.Distance(lastPlayerPos, monster.transform.position);
        if (!isMiss && distance <= monster.AttackDistance)
        {
            // 공격 범위 안에 있으면 공격
            fsm.SetState(new AttackState(monster));
        }
        else
        {
            // 공격 범위 밖이면 계속해서 플레이어 추적
            monster.MoveTo(lastPlayerPos);
            if (isArrive)
            {
                // 마지막으로 탐지된 플레이어의 좌표에 도달한 경우 탐지 상태로 전환
                // #임시로 일반 상태로 전환
                fsm.SetState(new IdleState(monster));
            }
        }
    }

    private void UpdatePlayerPos()
    {
        // 플레이어 탐지
        if (monster.GetPlayerPos() is Vector3 playerPos)
        {
            // 플레이어가 탐지 범위 안이면 새 좌표 갱신
            lastPlayerPos = playerPos;
        }
    }

    public void OnEnterState() { }

    public void OnTakeDamage(FSM fsm) { }
}