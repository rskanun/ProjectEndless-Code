using Endless.CustomObject;
using UnityEngine;

public class IdleState : IMonsterState
{
    private Monster monster;

    // 몬스터 이동 관련 변수
    private CircleLinkedList<Vector2> pointLinkedList; // 이동하게 될 목표 좌표 목록
    private Node<Vector2> curTargetPos; // 현재 목표 좌표
    private float thinkDelay;

    public IdleState(Monster monster)
    {
        this.monster = monster;
    }

    public void OnEnterState()
    {
        pointLinkedList = new CircleLinkedList<Vector2>(monster.MovePoints);

        // 첫 목표 설정
        curTargetPos = pointLinkedList.Head;
    }

    public void OnAction(FSM fsm)
    {
        if (monster.Propensity == Propensity.Hostile)
        {
            // 적대적 성향일 경우 플레이어 탐지
            Vector3 playerPos = monster.DetectPlayer();
            if (playerPos != monster.transform.position)
            {
                // 플레이어 추적 상태로 변경
                fsm.SetState(new ChaseState(monster));
            }
        }

        // idle action
        OnMove();
    }

    private void OnMove()
    {
        if (thinkDelay <= 0)
        {
            // targetPos 까지 움직임
            monster.MoveTo(curTargetPos.Value);

            // 목표(+오차) 도달 확인
            if (Vector2.Distance(curTargetPos.Value, monster.transform.position) <= 0.5f)
            {
                // 다음 포인트 설정
                curTargetPos = curTargetPos.Next;

                // 다음 행동까지 딜레이
                thinkDelay = Random.Range(1, 5);
            }
        }
        else
        {
            thinkDelay -= Time.deltaTime;
        }
    }

    public void OnTakeDamage(FSM fsm)
    {
        // 공격을 당할 시, 적대적인 성향을 띔
        monster.Propensity = Propensity.Hostile;

        // 플레이어 추적 상태로 변경
        fsm.SetState(new ChaseState(monster));
    }
}