using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterObject : MonoBehaviour
{
    [Header("참조 스크립트")]
    [SerializeField] private OrganManager organManager;

    [Header("구성 요소")]
    [ReadOnly, SerializeField] private Animator anim;

    [Header("몬스터 행동 정보")]
    // 이동 정보
    [SerializeField] private float moveSpeed;
    [SerializeField] private float chasingSpeed;
    [SerializeField] private Color moveLineColor;
    [SerializeField]
    private List<Vector2> movePoints;
    public List<Vector2> MovePoints { get { return movePoints; } }
    private bool _isMove;
    public bool IsMove
    {
        get { return _isMove; }
        set
        {
            _isMove = value;
            anim.SetBool("IsMove", value);
        }
    }
    private Vector3 prevPos;

    // 공격 정보
    [SerializeField]
    private float _attackDistance;
    public float AttackDistance
    {
        get { return _attackDistance; }
    }
    [ReadOnly, SerializeField]
    private bool _isAttacked;
    public bool IsAttacked
    {
        private set { _isAttacked = value; }
        get { return _isAttacked; }
    }

    // 몬스터 상태 정보
    private FSM fsm = new FSM();

#if UNITY_EDITOR
    private void OnValidate()
    {
        anim = GetComponent<Animator>();

        prevPos = transform.position;
    }
#endif

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
        // 공격 모션 실행
        IsAttacked = true;
        anim.SetTrigger("Attack");
    }

    public void OnEndMotion()
    {
        IsAttacked = false;
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

        // 현재 위치에서 목표까지 갈 수 있다면 이동 목표를 목적지로 설정
        movePoint = (cur2Move > cur2Target) ? target : movePoint;

        // 이동 방향으로 회전
        RotateTo(movePoint);

        // 목표로 이동
        transform.position = movePoint;
    }

    public void RotateTo(Vector2 target)
    {
        Vector2 dir = (target - (Vector2)transform.position).normalized;

        // 탐지 기관 회전
        organManager.RotateOrgans(dir);

        // 방향에 따른 애니메이션 변경
        SetMoveAnim(dir);
    }

    private void SetMoveAnim(Vector2 dir)
    {
        if (Mathf.Abs(dir.x) < Mathf.Abs(dir.y)) dir.x = 0;
        else dir.y = 0;

        int h = Mathf.RoundToInt(dir.x + 0.5f * Mathf.Sign(dir.x));
        int v = Mathf.RoundToInt(dir.y + 0.5f * Mathf.Sign(dir.y));

        anim.SetInteger("axisH", h);
        anim.SetInteger("axisV", v);
    }
}