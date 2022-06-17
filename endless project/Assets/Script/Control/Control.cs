using System.Collections;
using UnityEngine;

public class Control : MonoBehaviour
{
    private const float STOP_DISTANCE = 0.05f;

    private bool noMoveKeyDown = false;
    private bool isDash = false;

    private Vector2 vec;
    private Vector2 animaVec;
    private Vector2 dashVec;

    private Animator animator;

    private Rigidbody2D rigidbody;

    private Transform mainEntity; // 플레이어 캐릭터
    private Transform subEntity; // 대쉬 예상 지점 계산을 위한 가상의 엔티티

    private int speed;
    private int dashSpeed;
    private int dashConstant;

    /*
     *  Key Value
     */

    // 쓰지 않을 방향키
    private string left     = Option.getKey(Key.left);
    private string right    = Option.getKey(Key.right);
    private string up       = Option.getKey(Key.up);
    private string down     = Option.getKey(Key.down);

    // 대쉬키
    private string dash     = Option.getKey(Key.dash);

    private void Awake()
    {
        init();
    }

    private void init()
    {
        initComponent();

        // 그래픽 회전 방지
        rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;

        mainEntity = this.gameObject.transform;
    }

    private void initComponent()
    {
        animator        = GetComponent<Animator>();
        rigidbody       = GetComponent<Rigidbody2D>();
        speed           = GetComponent<Player>().speed;
        dashConstant    = GetComponent<Player>().dashConstant;
        dashSpeed       = GetComponent<Player>().dashSpeed;
    }

    private void Update()
    {
        // 움직임 키 감지
        if (!noMoveKeyDown)
            moveKeyPress();

        
    }

    /************************************************************
     * [키 제어]
     * 
     * 플레이어가 누르는 키(ex: 방향키)에 따른 움직임 제어
     ************************************************************/

    // 키 입력에 따른 움직임 제어
    private void moveKeyPress()
    {
        // 대쉬키
        if (Input.GetKeyDown(dash))
        {
            dashKey();
        }

        // 방향키
        if (!isOtherMove()) // 키 타입에 따른 입력방지
        {
            moveKey();
        }

        // 애니메이션 움직임 제어
        setAnimator();
    }

    private void setAnimator()
    {
        // 올림 보정
        int x = Mathf.CeilToInt(animaVec.x);
        int y = Mathf.CeilToInt(animaVec.y);

        animator.SetInteger("axisH", x);
        animator.SetInteger("axisV", y);
    }

    // 방향키 입력
    private void moveKey()
    {
        // 패드 및 키보드의 움직임(패드의 경우 경도)에 따른 백터 변화
        vec.x = Input.GetAxisRaw("Horizontal");
        vec.y = Input.GetAxisRaw("Vertical");

        animaVec = vec;
    }

    // 마우스 방향으로 빠르게 이동하는 대쉬키
    private void dashKey()
    {
        // 대쉬 중 움직임 금지
        noMoveKeyDown = true;

        // 대쉬 이동을 위한 계산식
        getDashVector();

        // 애니메이션 움직임 보정 및 설정
        int tmpX = (int)dashVec.x;
        int tmpY = (int)dashVec.y;

        animaVec.x = (-10 <= tmpX && tmpX <= 10) ? 0 : tmpX;
        animaVec.y = (-10 <= tmpY && tmpY <= 10) ? 0 : tmpY;

        // 대쉬 작동
        isDash = true;
    }

    private void getDashVector()
    {
        // 서브 엔티티를 플레이어로 이동
        subEntity.position = mainEntity.position;

        // 대쉬 벡터 초기화
        dashVec = Vector2.zero;

        // 캐릭터와 마우스 각도 계산
        Vector2 loc_click = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 loc_chr = mainEntity.position;

        float angle = Mathf.Atan2(loc_click.y - loc_chr.y, loc_click.x - loc_chr.x)
            * Mathf.Rad2Deg;

        // 이동 거리 계산
        int distance = speed * dashConstant;

        dashVec.x = Mathf.Cos(angle * Mathf.Deg2Rad) * distance;
        dashVec.y = Mathf.Sin(angle * Mathf.Deg2Rad) * distance;

        dashVec.x += loc_chr.x;
        dashVec.y += loc_chr.y;
    }

    /************************************************************
    * [시스템]
    * 
    * 실제 게임 내의 캐릭터의 움직임이나 모션을 제어
    ************************************************************/

    private void FixedUpdate()
    {
        // 대쉬 발동에 따른 이동
        if (isDash)
        {
            // 캐릭터 및 서브 오브젝트 이동
            rigidbody.MovePosition(Vector2.Lerp(mainEntity.position, dashVec, dashSpeed));
            subEntity.position = Vector2.Lerp(subEntity.position, dashVec, dashSpeed);

            // 만약 두 오브젝트 사이가 일정 수준까지 가까워지면 이동제한 해제
            if (Vector2.Distance(dashVec, subEntity.position) <= STOP_DISTANCE)
            {
                Debug.Log("ready");
                isDash = false;
                noMoveKeyDown = false;
            }
        }

        // 방향키 이동에 따른(혹은 그에 준하는) 이동
        rigidbody.velocity = vec.normalized * speed;
    }

    private bool isOtherMove()
    {
        return Input.GetKey(up) || Input.GetKey(down)
            || Input.GetKey(left) || Input.GetKey(right);
    }

}