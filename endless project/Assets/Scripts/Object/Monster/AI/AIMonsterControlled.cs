using UnityEngine;

// 성향: 플레이어와 대면 했을 때의 행동
public enum Propensity
{
    Friendly,   // 플레이어에게 이로운 영향을 주지만, 피해를 입을 시 적대적으로 변함
    Neutral,    // 플레이어에게 아무런 영향을 끼치지 않으나, 피해를 입을 시 적대적으로 변함
    Hostile     // 플레이어에게 해로운 영향을 끼침
}
// 적대적일 때 취하는 행동
public enum Personality
{
    Bravery,    // 자신의 체력에 상관없이 공격만을 행함
    Prudence,   // 자신의 체력에 따라 공격을 행하거나 수비적인 태세를 취함
    Skittish    // 플레이어로부터 일정 거리 이상까지 도망침
}
// 다른 개체들과의 행동
public enum Quality
{
    Independent,    // 같은 개체에 영향을 받지 않음
    Social,         // 동일한 개체끼리 다니며, 한 마리라도 적대적으로 변하면 주변의 동일한 개체들도 적대적으로 변함
    Protective      // 적대적으로 변할 시 주변에 있는 모든 개체들도 적대적으로 변함
}
public class AIMonsterControlled : MonoBehaviour
{
    [Header("몬스터 AI")]
    // 몬스터 성향
    [SerializeField]
    private Propensity propensity; // 기본 성향
    private Propensity currentPropensity; // 현재 성향

    // 몬스터 성격
    [SerializeField]
    private Personality personality;

    // 몬스터 개체 특성
    [SerializeField]
    private Quality quality;

    // 몬스터 이동 관련 변수
    [Header("이동 반경")]
    [SerializeField] private float moveRadius;
    [ReadOnly] private float thinkDelay;
    [ReadOnly] private Vector2 targetPos; // 움직일 위치
    [ReadOnly] private float prevVelocity;

    // 플레이어 탐지 반경
    [Header("플레이어 탐지 반경")]
    [SerializeField] private float detectionArea;

    // 참조 컴포넌트
    private Rigidbody2D rigid;

    // 참조 스크립트
    private Monster monster;

    private void Awake()
    {
        rigid = gameObject.GetComponent<Rigidbody2D>();
        monster = gameObject.GetComponent<Monster>();
    }

    private void OnEnable()
    {
        currentPropensity = propensity;
    }

    public void OnTakeDamage()
    {
        if (currentPropensity != Propensity.Hostile)
        {
            // 공격을 당할 시, 적대적인 성향을 띔
            currentPropensity = Propensity.Hostile;
        }
    }

    /***************************************************************
    * [ 기본(Idle) 상태 ]
    * 
    * 자유롭게 행동하는 상태
    ***************************************************************/

    public void OnEnterIdle()
    {
        ThinkMoveVec();
    }

    public void OnIdleAction()
    {
        if (thinkDelay <= 0)
        {
            if (currentPropensity == Propensity.Hostile)
            {
                // 적대적 성향일 경우 플레이어 탐지
                DetectPlayer();
            }

            prevVelocity = rigid.velocity.magnitude;
            Debug.Log(prevVelocity + "/" + rigid.velocity.magnitude);

            // targetPos 까지 움직임
            Vector2 moveVec = targetPos - (Vector2)transform.position;
            float speed = monster.Stat.MoveSpeed * Time.deltaTime;
            
            Vector2 velocity = moveVec.normalized * speed;

            if (rigid.velocity.magnitude >= velocity.magnitude || prevVelocity == 0)
            {
                rigid.velocity = moveVec.normalized * speed;

                // 목표 좌표 도달 확인
                if (Vector2.Distance(targetPos, transform.position) <= 1f)
                {
                    prevVelocity = 0;

                    ThinkMoveVec();
                }
            }
            else
            {
                // 움직이던 도중 멈출 경우

                //ThinkMoveVec();
                Debug.Log("stop");
            }
        }
        else
        {
            thinkDelay -= Time.deltaTime;
        }
    }

    private void DetectPlayer()
    {
        // 플레이어 탐지
        Vector3 playerVec = GetPlayerPos();
        if (playerVec != transform.position)
        {
            targetPos = playerVec;

            // 플레이어 추적 상태로 변경
            monster.SetState(ChaseState.Instance);
        }
    }

    private void ThinkMoveVec()
    {
        // 움직임 정지
        rigid.velocity = Vector2.zero;

        // 이동할 위치 설정
        float movePosX = Random.Range(-moveRadius, moveRadius) + transform.position.x;
        float movePosY = Random.Range(-moveRadius, moveRadius) + transform.position.y;

        targetPos = new Vector2(movePosX, movePosY);

        // 다음 행동까지 딜레이
        thinkDelay = Random.Range(1, 5);
    }

    private Vector2 GetPlayerPos()
    {
        // 범위 내 모든 오브젝트 탐지
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionArea);
        foreach (Collider collider in colliders)
        {
            // 플레이어를 탐지한 경우 리턴
            if (collider.CompareTag("Player"))
            {
                return collider.transform.position;
            }
        }

        return transform.position;
    }
}