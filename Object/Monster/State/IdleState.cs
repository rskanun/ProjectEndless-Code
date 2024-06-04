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
        // 성향에 따른 idle 액션
        monster.CurPropensity.OnIdleAction(fsm);

        // 공통된 idle 움직임 액션
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
        monster.CurPropensity.OnAttacked(fsm);
    }
}