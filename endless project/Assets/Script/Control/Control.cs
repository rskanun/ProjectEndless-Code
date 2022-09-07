using System.Collections;
using UnityEngine;

public class Control : MonoBehaviour
{
    private const float STOP_DISTANCE = 0.05f;

    private const int ANIMATION_CONSTANT = 10;

    private bool noMoveKeyDown = false;
    private bool isDash = false;
    private bool isTalking = false;

    private bool dashing = false;
    private bool npcInArea = false;

    private Vector2 vec;
    private Vector2 dashVec;

    private Animator animator;

    private Rigidbody2D rigid;
    public GameObject subRigid;

    private Transform mainEntity; // 플레이어 캐릭터
    private Transform subEntity; // 대쉬 예상 지점 계산을 위한 가상의 엔티티

    private TextManager text;
    private NPC npc; // 상호작용 할 npc

    [SerializeField]
    private Player player;

    /************************************************************
     * [Key Value]
     * 
     * 각종 키들의 string을 모아둔 변수
     ************************************************************/

    // 쓰지 않을 방향키
    private string left     = Option.getKey(Key.left);
    private string right    = Option.getKey(Key.right);
    private string up       = Option.getKey(Key.up);
    private string down     = Option.getKey(Key.down);

    // 대쉬키
    private string dash     = Option.getKey(Key.dash);

    // 상호작용키
    private string interact = Option.getKey(Key.interact);

    // 액션키
    private string action   = Option.getKey(Key.action);

    private void Awake()
    {
        init();
    }

    private void init()
    {
        initComponent();

        // 그래픽 회전 방지
        rigid.constraints = RigidbodyConstraints2D.FreezeRotation;

        mainEntity  = this.gameObject.transform;
        subEntity   = subRigid.transform;
    }

    private void initComponent()
    {
        animator        = GetComponent<Animator>();
        rigid           = GetComponent<Rigidbody2D>();
        text            = GetComponent<TextManager>();
    }

    private void Update()
    {
        // 텍스트 상호작용 키 감지
        // 대화가능한 npc가 범위 내에 있다면 상호작용 키를,
        // 대화 도중이라면 액션키를 감지
        if (npcInArea && Input.GetKeyDown(interact) || isTalking && Input.GetKeyDown(action))
            moveBan(ref isTalking, text.talk(npc));

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
        // 방향키
        if (!isOtherMove()) // 키 타입에 따른 입력방지
        {
            moveKey();
        }

        // 대쉬키
        if (Input.GetKeyDown(dash))
        {
            dashKey();
        }
    }

    private void setAnimator(Vector2 vec)
    {
        // 올림 보정
        int x = Mathf.CeilToInt(vec.x);
        int y = Mathf.CeilToInt(vec.y);

        // 애니메이션 움직임 제어
        animator.SetInteger("axisH", x);
        animator.SetInteger("axisV", y);
    }

    // 방향키 입력
    private void moveKey()
    {
        // 패드 및 키보드의 움직임(패드의 경우 경도)에 따른 백터 변화
        vec.x = Input.GetAxisRaw("Horizontal");
        vec.y = Input.GetAxisRaw("Vertical");

        // 키보드 누른 방향으로 애니메이션 움직임 제어
        setAnimator(vec);
    }

    // 마우스 방향으로 빠르게 이동하는 대쉬키
    private void dashKey()
    {
        // 대쉬 중 움직임 금지
        moveBan(ref isDash, true);

        // 대쉬 이동을 위한 계산식
        getDashVector();

        // 대쉬 작동
        dashing = true;
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
        float distance = player.speed * player.DashConstant;

        dashVec.x = Mathf.Cos(angle * Mathf.Deg2Rad) * distance;
        dashVec.y = Mathf.Sin(angle * Mathf.Deg2Rad) * distance;

        setDashAnimation();

        dashVec.x += loc_chr.x;
        dashVec.y += loc_chr.y;
    }

    private void setDashAnimation()
    {
        // 애니메이션 움직임 보정 및 설정
        int tmpX = (int)dashVec.x;
        int tmpY = (int)dashVec.y;

        Vector2 animaVec = new Vector2();

        animaVec.x = (-ANIMATION_CONSTANT <= tmpX && tmpX <= ANIMATION_CONSTANT) ? 0 : tmpX;
        animaVec.y = (-ANIMATION_CONSTANT <= tmpY && tmpY <= ANIMATION_CONSTANT) ? 0 : tmpY;

        setAnimator(animaVec);
    }

    /************************************************************
    * [시스템]
    * 
    * 실제 게임 내의 캐릭터의 움직임이나 모션을 상황에 맞게 제어
    ************************************************************/

    private void FixedUpdate()
    {
        // 대쉬 발동에 따른 이동
        if (dashing)
        {
            // 캐릭터 및 서브 오브젝트 이동
            rigid.MovePosition(Vector2.Lerp(mainEntity.position, dashVec, player.DashSpeed));
            subEntity.position = Vector2.Lerp(subEntity.position, dashVec, player.DashSpeed);

            // 만약 두 오브젝트 사이가 일정 수준까지 가까워지면 이동제한 해제
            if (Vector2.Distance(dashVec, subEntity.position) <= STOP_DISTANCE)
            {
                Debug.Log("ready");
                dashing = false;
                moveBan(ref isDash, false);
            }
        }

        // 방향키 이동에 따른(혹은 그에 준하는) 이동
        rigid.velocity = vec.normalized * player.speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 맞닿은 오브젝트가 NPC일 시
        if (collision.CompareTag("NPC"))
        {
            // 해당 NPC의 정보를 가져오고
            npc = collision.gameObject.GetComponent<NPC>();

            // npcInArea를 활성화
            npcInArea = true;
            Debug.Log("keydown " + interact);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // 맞닿은 오브젝트가 NPC일 시
        if (collision.CompareTag("NPC"))
        {
            npc = null;

            npcInArea = false;
            Debug.Log("exit");
        }
    }

    private bool isOtherMove()
    {
        // 키보드의 경우 wasd와 방향키가 모두 먹히게 되는데,
        // 마우스를 사용하는 모드의 경우 방향키를
        // 마우스를 사용하지 않는 모드의 경우 wasd의 입력을 막는다.
        return Input.GetKey(up) || Input.GetKey(down)
            || Input.GetKey(left) || Input.GetKey(right);
    }

    private void moveBan(ref bool getNoMoveBool, bool isStop)
    {
        // 움직임 제어 bool 상태 변경
        getNoMoveBool = isStop;

        // 움직임을 제어하는 bool이 하나라도 존재한다면 움직임을 막음
        noMoveKeyDown = isDash || isTalking;
    }

}