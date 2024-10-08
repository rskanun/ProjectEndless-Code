using UnityEngine;

public class IdleState : IMonsterState
{
    private MonsterObject monster;

    // 몬스터 이동 관련 변수
    private Vector2 targetPos;
    private int pointIndex;
    private float thinkDelay;

    public IdleState(MonsterObject monster)
    {
        this.monster = monster;
    }

    public void OnEnterState()
    {
        // 첫 목표 설정
        targetPos = monster.MovePoints[pointIndex++];
    }

    public void OnAction(FSM fsm)
    {
        OnDetected(fsm);
        OnMove();
    }

    private void OnDetected(FSM fsm)
    {
        Vector3 playerPos = monster.DetectPlayer();
        if (playerPos != monster.transform.position)
        {
            // 탐지에 성공하면 플레이어 추적
            fsm.SetState(new ChaseState(monster));
        }
    }

    private void OnMove()
    {
        if (thinkDelay <= 0)
        {
            // targetPos 까지 움직임
            monster.MoveTo(targetPos);

            // 목표(+오차) 도달 확인
            if (Vector2.Distance(targetPos, monster.transform.position) <= 0.5f)
            {
                // 다음 포인트 설정
                targetPos = monster.MovePoints[pointIndex++];
                if (pointIndex >= monster.MovePoints.Count)
                {
                    pointIndex = 0;
                }

                // 다음 행동까지 딜레이
                thinkDelay = Random.Range(1, 5);
            }
        }
        else
        {
            thinkDelay -= Time.deltaTime;
        }
    }

    public void OnTakeDamage(FSM fsm) { }
}