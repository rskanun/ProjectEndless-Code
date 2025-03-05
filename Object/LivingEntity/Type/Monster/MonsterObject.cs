using System.Collections.Generic;
using UnityEngine;

public class MonsterObject : MonoBehaviour
{
    [Header("몬스터 행동 정보")]
    // 이동 정보
    [SerializeField] private float moveSpeed;
    [SerializeField] private float chasingSpeed;
    [SerializeField] private Color moveLineColor;
    [SerializeField]
    private List<Vector2> movePoints;
    public List<Vector2> MovePoints { get { return movePoints; } }

    // 공격 정보
    [SerializeField]
    private float _attackDistance;
    public float AttackDistance
    {
        get { return _attackDistance; }
    }
    [SerializeField]
    private float _attackCooldown;
    public float AttackCooldown
    {
        get { return _attackCooldown; }
    }

    [Header("참조 스크립트")]
    [SerializeField] private OrganManager organManager;

    // 몬스터 상태 정보
    private FSM fsm = new FSM();

    private void OnEnable()
    {
        // idle 상태 초기화
        fsm.SetState(new IdleState(this));
    }

    private void FixedUpdate()
    {
        fsm.OnAction();
    }

    private void OnDrawGizmos()
    {
        // 이동 루트
        if (movePoints.Count > 0)
        {
            Gizmos.color = moveLineColor;

            Vector2 prevPos = movePoints[movePoints.Count - 1];
            foreach (Vector2 pos in movePoints)
            {
                Gizmos.DrawLine(prevPos, pos);

                prevPos = pos;
            }
        }
    }

    /***************************************************************
    * [ 플레이어 탐지 ]
    * 
    * 탐지 기관을 통한 플레이어 탐지
    ***************************************************************/

    public Vector3? GetPlayerPos()
    {
        return organManager.DetectPlayer();
    }

    /***************************************************************
    * [ 몬스터 상태 처리 ]
    * 
    * 몬스터의 공격 처리
    ***************************************************************/

    public virtual void OnAttack()
    {
        // 전투 돌입
    }

    /***************************************************************
    * [ 몬스터 이동 ]
    * 
    * 몬스터 이동에 따른 위치 및 애니메이션 변화 처리
    ***************************************************************/

    public void MoveTo(Vector2 target)
    {
        float speed = moveSpeed * Time.deltaTime;

        // 현재 위치에서 움직일 위치와 목표까지의 거리 계산
        Vector2 movePoint = Vector2.MoveTowards(transform.position, target, speed);
        float cur2Move = Vector2.Distance(transform.position, movePoint); // 현재 좌표에서 움직인 뒤까지의 
        float cur2Target = Vector2.Distance(transform.position, target); // 현재 좌표에서 목표까지의 거리

        // 현재 위치에서 목표까지 갈 수 있다면 목표로 이동
        transform.position = (cur2Move > cur2Target) ? target : movePoint;

        // 이동 방향으로 몸 회전
        RotateTo(movePoint);
    }

    public void RotateTo(Vector2 target)
    {
        Vector2 rotateVec = (target - (Vector2)transform.position).normalized;

        // 탐지 기관 회전
        organManager.RotateOrgans(rotateVec);
    }
}