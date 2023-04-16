using Assets.Script.Control.Text;
using Assets.Script.System;
using System.Collections;
using UnityEngine;

public class ChrControl : MonoBehaviour
{
    // 상호작용 할 npc
    private NPC npc;

    [SerializeField]
    private PlayerData player;

    [Header("참조 스크립트")]
    public LineManager lineManager;

    private OptionSetting option;
    private NoKeyDown noKeyDown;

    private const int ANIMATION_CONSTANT = 10; // 플레이어의 마우스 위치에 따른 시선 변경 보정 상수
    private const float STOP_DISTANCE = 0.05f;

    private Vector2 playerVec;
    private Vector2 dashVec;

    private Animator moveAnimator;
    private Rigidbody2D rigid;
    private Transform mainEntity; // 플레이어 캐릭터
    public Transform subEntity; // 대쉬 예상 지점 계산을 위한 가상의 엔티티

    private bool isMoveKey
    {
        get
        {
            return Input.GetKey(option.Up) || Input.GetKey(option.Down)
            || Input.GetKey(option.Left) || Input.GetKey(option.Right);
        }
    }

    private void Awake()
    {
        option = OptionSetting.Instance;
        noKeyDown = NoKeyDown.Instance;

        mainEntity = this.gameObject.transform;

        initComponent();
    }

    private void initComponent()
    {
        moveAnimator    = GetComponent<Animator>();
        rigid       = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // 텍스트 상호작용 키 감지
        // 자세한 코드는 Text -> TextManager
        if(noKeyDown.IsMenuActive == false)
        {
            talkingKeyPress();
        }

        // 움직임 키 감지
        if (noKeyDown.IsPlayerControllable)
        {
            moveKeyPress();
        }
    }

    /************************************************************
    * [대화 출력]
    * 
    * 인게임 화면의 대화 제어
    ************************************************************/

    public void talkingKeyPress()
    {
        // 대화의 첫 시작일 경우
        if (noKeyDown.IsTalking == false)
        {
            // 대화가능한 npc가 범위 내에 있다면 상호작용 키로 대화를 활성화
            if (npc is not null && Input.GetKeyDown(option.Interact))
            {
                lineManager.initTalk(npc);
            }
        }
    }

    /************************************************************
     * [움직임 제어]
     * 
     * 방향키에 따른 움직임 제어
     ************************************************************/

    // 키 입력에 따른 움직임 제어
    private void moveKeyPress()
    {
        // 방향키
        if (isMoveKey)
        {
            moveKey();
        }

        // 대쉬키
        if (Input.GetKeyDown(option.Dash))
        {
            dashKey();
        }
    }

    // 방향키 입력
    private void moveKey()
    {
        // 패드 및 키보드의 움직임(패드의 경우 경도)에 따른 백터 변화
        playerVec.x = Input.GetAxisRaw("Horizontal");
        playerVec.y = Input.GetAxisRaw("Vertical");

        // 키보드 누른 방향으로 애니메이션 움직임 제어
        setAnimator(playerVec);
    }

    private void setAnimator(Vector2 vec)
    {
        // 올림 보정
        int x = Mathf.CeilToInt(vec.x);
        int y = Mathf.CeilToInt(vec.y);

        // 애니메이션 움직임 제어
        moveAnimator.SetInteger("axisH", x);
        moveAnimator.SetInteger("axisV", y);
    }

    /************************************************************
    * [대쉬 키]
    * 
    * 마우스 방향으로 플레이어가 빠르게 이동
    ************************************************************/

    private void dashKey()
    {
        // 대쉬 이동을 위한 계산식
        getDashVector();

        // 대쉬 작동
        noKeyDown.IsDashing = true;
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
        float distance = player.Speed;

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
        if (noKeyDown.IsDashing)
        {
            moveDash();
        }

        // 움직일 수 없는 상태이면 이동을 멈춤
        if(noKeyDown.IsPlayerControllable == false)
        {
            rigid.velocity = new Vector2(0, 0);
        }
        // 방향키 이동에 따른(혹은 그에 준하는) 이동
        else
            rigid.velocity = playerVec.normalized * player.Speed;
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
            noKeyDown.IsDashing = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 맞닿은 오브젝트가 NPC일 시
        if (collision.CompareTag("NPC"))
        {
            // 해당 NPC의 정보를 가져오기
            npc = collision.gameObject.GetComponent<NPC>();
            Debug.Log("keydown " + option.Interact.ToString());
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
}