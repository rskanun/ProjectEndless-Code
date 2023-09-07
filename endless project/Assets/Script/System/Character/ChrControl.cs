using Assets.Script.Control.Text;
using Assets.Script.System;
using System.Collections;
using UnityEngine;

public class ChrControl : MonoBehaviour
{
    // 플레이어의 마우스 위치에 따른 시선 변경 보정 상수
    private const int ANIMATION_CONSTANT = 10;
    
    // 대쉬 제동 거리
    private const float MIN_STOP_DISTANCE = 0.05f;
    private float stopDistance = MIN_STOP_DISTANCE;

    // 플레이어와 대쉬 보조용 가상 엔티티 벡터
    private Vector2 playerVec;
    private Vector2 dashVec;

    private Animator playerAnimator;
    private Rigidbody2D rigid;
    private Transform mainEntity; // 플레이어 캐릭터
    public Transform subEntity; // 대쉬 예상 지점 계산을 위한 가상의 엔티티

    [Header("플레이어 데이터")]
    [SerializeField]
    private PlayerData player;

    // 참조 스크립터블 오브젝트
    private OptionSetting option;
    private PlayerState playerState;

    private bool isPlayerRunningStopped
    {
        get
        {
            return playerState.IsRunning && playerVec.Equals(Vector2.zero);
        }
    }

    private void Awake()
    {
        option = OptionSetting.Instance;
        playerState = PlayerState.Instance;

        mainEntity = gameObject.transform;

        player.OnPlayerAngleChanged.AddListener(OnPlayerAngleChanged);

        initComponent();
    }

    private void initComponent()
    {
        playerAnimator      = GetComponent<Animator>();
        rigid               = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // 움직임 키 감지
        if (playerState.IsPlayerControllable)
        {
            moveKeyPress();
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
        moveActionKey();

        // 대쉬키
        if (Input.GetKeyDown(option.Dash))
        {
            dashKey();
        }
    }

    // 방향키 입력
    private void moveActionKey()
    {
        // 패드 및 키보드의 움직임(패드의 경우 경도)에 따른 백터 변화
        playerVec.x = Input.GetAxis("Horizontal");
        playerVec.y = Input.GetAxis("Vertical");

        // 키보드 누른 방향으로 애니메이션 움직임 제어
        player.Angle = playerVec;
    }

    private void OnPlayerAngleChanged(Vector2 angle)
    {
        // 올림 보정
        int x = Mathf.CeilToInt(angle.x);
        int y = Mathf.CeilToInt(angle.y);

        // 애니메이션 움직임 제어
        playerAnimator.SetInteger("axisH", x);
        playerAnimator.SetInteger("axisV", y);
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

        // 멈춤 거리 계산
        stopDistance = player.RunSpeed * Time.fixedDeltaTime;

        // 대쉬 작동
        playerState.IsDashing = true;

        // 달리기 멈춤 체크
        StartCoroutine(checkPlayerRunningStopped());
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

        // 대쉬 애니메이션 설정
        setDashAnimation();

        // 현재 좌표값을 기준으로 대쉬 좌표 재설정
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

        player.Angle = animaVec;
    }

    IEnumerator checkPlayerRunningStopped()
    {
        WaitForSeconds checkTime = new WaitForSeconds(0.2f);

        while (playerState.IsRunning)
        {
            // 대쉬 중이 아닌 플레이어가 달리는 걸 멈출 경우
            if (playerState.IsDashing == false && isPlayerRunningStopped)
            {
                // 달리기 종료
                playerState.IsRunning = false;
                Debug.Log("stop!");
            }

            yield return checkTime;
        }
    }

    /************************************************************
    * [물리 시스템]
    * 
    * 실제 게임 내의 캐릭터의 행동에 따른 변화
    ************************************************************/

    private void FixedUpdate()
    {
        // 대쉬 발동에 따른 이동
        if (playerState.IsDashing)
        {
            moveDash();
        }

        // 방향키(혹은 비슷한 장치) 이동에 따른 이동
        moveCharacter();
    }

    private void moveDash()
    {
        // 캐릭터 및 서브 오브젝트 이동
        rigid.MovePosition(Vector2.Lerp(mainEntity.position, dashVec, player.DashSpeed));
        subEntity.position = Vector2.Lerp(subEntity.position, dashVec, player.DashSpeed);

        // 만약 두 오브젝트 사이가 일정 수준까지 가까워지면 이동제한 해제
        if (Vector2.Distance(dashVec, subEntity.position) <= stopDistance)
        {
            // 달리기 거리만큼 가까워졌음에도 움직이지 않고 있을 경우
            if(isPlayerRunningStopped)
            {
                // 달리기 멈춤
                playerState.IsRunning = false;

                // 제동 거리 재설정
                stopDistance = MIN_STOP_DISTANCE;

                Debug.Log("stop!");
            }
            else
            {
                playerState.IsDashing = false;

                Debug.Log("ready");
            }
        }
    }

    private void moveCharacter()
    {
        // 움직일 수 없는 상태이면 이동을 멈춤
        if (playerState.IsPlayerControllable == false)
        {
            rigid.velocity = new Vector2(0, 0);
        }
        else
        {
            // 달리고 있는 경우의 속도
            if (playerState.IsRunning)
            {
                rigid.velocity = playerVec.normalized * player.RunSpeed;
            }
            // 걷고 있는 경우의 속도
            else
            {
                rigid.velocity = playerVec.normalized * player.Speed;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 맞닿은 오브젝트가 NPC일 시
        if (collision.CompareTag("NPC"))
        {
            // 해당 NPC의 정보를 가져오기
            player.Npc = collision.gameObject.GetComponent<NPC>();
            Debug.Log("keydown " + option.Interact.ToString());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // 맞닿은 오브젝트가 NPC일 시
        if (collision.CompareTag("NPC"))
        {
            // NPC의 정보를 초기화
            player.Npc = null;
            Debug.Log("exit");
        }
    }
}