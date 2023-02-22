using Assets.Script.Control.Text;
using UnityEngine;

public class ChrControl : MonoBehaviour
{
    [SerializeField]
    private Player player;

    [Header("참조 스크립트")]
    public LineManager lineManager;

    [Space]
    [Header("참조 오브젝트")]
    public GameObject menuUI;

    // 캐릭터를 움직이는 모든 키 차단
    private bool noMoveKeyDown
    {
        get { return lineManager.IsTalking || isDashing || menuUI.activeSelf; }
    }

    // 옵션(ESC) 키 차단
    private bool noOptionKeyDown
    {
        get { return lineManager.IsTalking; }
    }

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
        animator    = GetComponent<Animator>();
        rigid       = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // 텍스트 상호작용 키 감지
        // 자세한 코드는 Text -> TextManager
        if(menuUI.activeSelf == false)
        {
            talkingKeyPress();
        }

        // 움직임 키 감지
        if (!noMoveKeyDown)
        {
            moveKeyPress();
        }
    }

    /************************************************************
    * [대화 출력]
    * 
    * 인게임 화면의 대화 제어
    ************************************************************/

    private NPC npc; // 상호작용 할 npc

    public void talkingKeyPress()
    {
        // 대화의 첫 시작일 경우
        if (!lineManager.IsTalking)
        {
            // 대화가능한 npc가 범위 내에 있다면 상호작용 키로 대화를 활성화
            if (npc is not null && Input.GetKeyDown(OptionSetting.Instance.interact))
            {
                lineManager.initTalk(npc);
            }
        }
    }

    /************************************************************
     * [움직임 제어]
     * 
     * 플레이어가 누르는 키(ex: 방향키)에 따른 움직임 제어
     ************************************************************/

    private const int ANIMATION_CONSTANT = 10;

    private Vector2 vec;

    private Animator animator;

    private Rigidbody2D rigid;
    public GameObject subRigid;

    // 키 입력에 따른 움직임 제어
    private void moveKeyPress()
    {
        // 방향키
        if (!isOtherMove()) // 키 타입에 따른 입력방지
        {
            moveKey();
        }

        // 대쉬키
        if (Input.GetKeyDown(OptionSetting.Instance.dash))
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

    /************************************************************
    * [대쉬 키]
    * 
    * 마우스 방향으로 플레이어가 빠르게 이동
    ************************************************************/

    private const float STOP_DISTANCE = 0.05f;

    private bool isDashing = false;

    private Vector2 dashVec;

    private Transform mainEntity; // 플레이어 캐릭터
    private Transform subEntity; // 대쉬 예상 지점 계산을 위한 가상의 엔티티

    private void dashKey()
    {
        // 대쉬 이동을 위한 계산식
        getDashVector();

        // 대쉬 작동
        isDashing = true;
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
        if (isDashing)
        {
            moveDash();
        }

        // 움직일 수 없는 상태이면 이동을 멈춤
        if(noMoveKeyDown)
        {
            rigid.velocity = new Vector2(0, 0);
        }
        // 방향키 이동에 따른(혹은 그에 준하는) 이동
        else
            rigid.velocity = vec.normalized * player.speed;
    }

    private void moveDash()
    {
        // 캐릭터 및 서브 오브젝트 이동
        rigid.MovePosition(Vector2.Lerp(mainEntity.position, dashVec, player.DashSpeed));
        subEntity.position = Vector2.Lerp(subEntity.position, dashVec, player.DashSpeed);

        // 만약 두 오브젝트 사이가 일정 수준까지 가까워지면 이동제한 해제
        if (Vector2.Distance(dashVec, subEntity.position) <= STOP_DISTANCE)
        {
            Debug.Log("ready");
            isDashing = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 맞닿은 오브젝트가 NPC일 시
        if (collision.CompareTag("NPC"))
        {
            // 해당 NPC의 정보를 가져오기
            npc = collision.gameObject.GetComponent<NPC>();
            Debug.Log("keydown " + OptionSetting.Instance.interact.ToString());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // 맞닿은 오브젝트가 NPC일 시
        if (collision.CompareTag("NPC"))
        {
            // NPC의 정보를 초기화
            npc = null;
            Debug.Log("exit");
        }
    }

    private bool isOtherMove()
    {
        // 키보드의 경우 wasd와 방향키가 모두 먹히게 되는데,
        // 마우스를 사용하는 모드의 경우 방향키를
        // 마우스를 사용하지 않는 모드의 경우 wasd의 입력을 막는다.
        return Input.GetKey(OptionSetting.Instance.up) || Input.GetKey(OptionSetting.Instance.down)
            || Input.GetKey(OptionSetting.Instance.left) || Input.GetKey(OptionSetting.Instance.right);
    }
}