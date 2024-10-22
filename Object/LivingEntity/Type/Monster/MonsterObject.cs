using System.Collections.Generic;
using UnityEngine;

// 몬스터 AI = 성향 + 탐지 기관 + 성격 + 개체 특성

// 성향: 플레이어와 대면 했을 때의 행동
public enum PropensityType
{
    Friendly,   // 플레이어에게 이로운 영향을 주지만, 피해를 입을 시 적대적으로 변함
    Neutral,    // 플레이어에게 아무런 영향을 끼치지 않으나, 피해를 입을 시 적대적으로 변함
    Hostile     // 플레이어에게 해로운 영향을 끼침
}
// 개체 특성: 다른 개체들과의 행동
public enum QualityType
{
    Independent,    // 같은 개체에 영향을 받지 않음
    Social,         // 동일한 개체끼리 다니며, 한 마리라도 적대적으로 변하면 주변의 동일한 개체들도 적대적으로 변함
    Protective      // 적대적으로 변할 시 주변에 있는 모든 개체들도 적대적으로 변함
}

public class MonsterObject : MonoBehaviour
{
    [Header("몬스터 성향")]
    [SerializeField] private PropensityType propensityType;

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

    public Vector3 DetectPlayer()
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

    public void MoveTo(Vector2 movePoint)
    {
        // movePoint를 향해 이동
        float speed = moveSpeed * Time.deltaTime;

        transform.position = Vector2.MoveTowards(transform.position, movePoint, speed);

        // 이동 방향으로 몸 회전
        RotateTo(movePoint);
    }

    public void RotateTo(Vector2 rotatePoint)
    {
        Vector2 rotateVec = (rotatePoint - (Vector2)transform.position).normalized;

        // 탐지 기관 회전
        organManager.RotateOrgans(rotateVec);
    }
}