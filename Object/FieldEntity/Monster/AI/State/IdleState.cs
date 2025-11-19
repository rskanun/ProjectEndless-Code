using UnityEngine;

public class IdleState : IMonsterState
{
    private FieldMonster monster;

    // 몬스터 이동 관련 변수
    private Vector2 targetPos;
    private int pointIndex;
    private float thinkDelay;

    public IdleState(FieldMonster monster)
    {
        this.monster = monster;
    }

    public void OnEnterState()
    {
        // 첫 이동 목표지 설정
        targetPos = monster.MovePoints[pointIndex++];
    }

    public void OnAction(FSM fsm)
    {
        // 주위 플레이어가 있는 지 탐지
        OnDetected(fsm);

        // 다음 포인트까지 이동 후 딜레이 가지기
        if (thinkDelay <= 0) OnMove();
        else OnPassedDelay();
    }

    private void OnDetected(FSM fsm)
    {
        if (monster.DetectPlayerPos() != null)
        {
            // 탐지에 성공하면 플레이어 추적
            fsm.SetState(new ChaseState(monster));
        }
    }

    private void OnMove()
    {
        // targetPos 까지 움직임
        monster.MoveTo(targetPos);

        // 목표 도달 확인
        if (targetPos == (Vector2)monster.transform.position)
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

    private void OnPassedDelay()
    {
        thinkDelay -= Time.deltaTime;
    }

    public void OnTakeDamage(FSM fsm) { }
}