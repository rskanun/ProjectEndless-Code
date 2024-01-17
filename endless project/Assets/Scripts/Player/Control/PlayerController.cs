using UnityEngine;

enum PlayerState
{
    Idle,
    Dashing,
    Attacking
}

public class PlayerController : MonoBehaviour, IControlState
{
    private int dashDistance = 40;
    private float stopDistance = 0.05f;

    // 현재 플레이어 캐릭터 상태
    private PlayerState playerState;
    private bool isRunning = false;
    private bool isStateIdle
    {
        get { return playerState == PlayerState.Idle; }
    }

    // 이동 위치 벡터
    private Vector2 playerVec;
    private Vector2 dashVec;

    // 현재 상호작용 가능한 NPC
    private Npc npc;

    [Header("관련 오브젝트")]
    [SerializeField] private Rigidbody2D rigid;
    [SerializeField] private Transform mainEntity; // 플레이어 캐릭터
    [SerializeField] private Transform subEntity; // 대쉬 예상 지점 계산을 위한 가상의 엔티티
    [SerializeField] private PlayerData player;

    [Header("참조 스크립트")]
    [SerializeField] private PlayerAnimation playerAnima;
    [SerializeField] private AttackManager attackManager;
    [SerializeField] private TalkController talkController;
    [SerializeField] private MenuController menuController;

    private void Start()
    {
        ControlContext.Instance.SetState(this);
    }

    public void OnControlKeyPressed()
    {
        OnMoveKeyPressed();
        OnRunKeyPressed();
        OnDashKeyPressed();
        OnAttackKeyPressed();
        OnTalkKeyPressed();
        OnMenuKeyPressed();
    }

    /************************************************************
     * [이동키]
     * 
     * 플레이어의 이동을 제어
     ************************************************************/

    private void OnMoveKeyPressed()
    {
        // 패드 및 키보드의 움직임(패드의 경우 경도)에 따른 백터 변화
        playerVec.x = Input.GetAxisRaw("Horizontal");
        playerVec.y = Input.GetAxisRaw("Vertical");

        // 걷는 정도의 스피드인지 판단
        CheckingWalk(playerVec.x, playerVec.y);

        // 키보드 누른 방향으로 애니메이션 움직임 제어
        SetAngle(playerVec);
    }

    private void SetAngle(Vector2 playerVec)
    {
        if (isStateIdle)
        {
            playerAnima.SetPlayerAngleAnim(playerVec);
        }
    }

    private void CheckingWalk(float x, float y)
    {
        if (isStateIdle)
        {
            float absX = Mathf.Abs(x);
            float absY = Mathf.Abs(y);

            bool isWalkSpeed = absX <= 0.5f && absY <= 0.5f;

            // 움직임 정도가 일정 이하면 걷기
            if (!isRunning && isWalkSpeed)
            {
                isRunning = false;
            }
            else if(!isRunning)
            {
                isRunning = true;
            }
        }
    }

    /************************************************************
     * [달리기 키]
     * 
     * 플레이어의 달리기를 제어
     ************************************************************/

    private void OnRunKeyPressed()
    {
        if(isStateIdle && Input.GetButtonDown("Running"))
        {
            isRunning = true;
        }
    }

    /************************************************************
    * [대시키]
    * 
    * 마우스 방향으로 플레이어가 빠르게 이동
    ************************************************************/

    private void OnDashKeyPressed()
    {
        if (isStateIdle && Input.GetButtonDown("Dash"))
        {
            // 대쉬 이동을 위한 계산식
            dashVec = getDashVector();

            // 움직임 멈춤
            rigid.velocity = Vector2.zero;

            // 대쉬 작동
            playerState = PlayerState.Dashing;
        }
    }

    private Vector2 getDashVector()
    {
        Vector2 vec = Vector2.zero;

        // 서브 엔티티를 플레이어로 이동
        subEntity.position = mainEntity.position;

        // 캐릭터와 마우스 각도 계산
        Vector2 locClick = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 locChr = mainEntity.position;

        float angle = Mathf.Atan2(locClick.y - locChr.y, locClick.x - locChr.x)
            * Mathf.Rad2Deg;

        // 이동 거리 계산
        vec.x = Mathf.Cos(angle * Mathf.Deg2Rad) * dashDistance;
        vec.y = Mathf.Sin(angle * Mathf.Deg2Rad) * dashDistance;

        // 대쉬 애니메이션 설정
        setDashAnimation(vec);

        // 현재 좌표값을 기준으로 대쉬 좌표 재설정
        vec.x += locChr.x;
        vec.y += locChr.y;

        return vec;
    }

    private void setDashAnimation(Vector2 moveVec)
    {
        // 애니메이션 움직임 보정 및 설정
        int constant = 10;
        int tmpX = (int)moveVec.x;
        int tmpY = (int)moveVec.y;

        Vector2 animaVec = new Vector2();

        animaVec.x = (-constant <= tmpX && tmpX <= constant) ? 0 : tmpX;
        animaVec.y = (-constant <= tmpY && tmpY <= constant) ? 0 : tmpY;

        playerAnima.SetPlayerAngleAnim(animaVec);
    }

    /************************************************************
    * [대화키]
    * 
    * 바라보는 대상과 대화 시작
    ************************************************************/

    private void OnTalkKeyPressed()
    {
        if(npc != null && isStateIdle && Input.GetButtonDown("Talking"))
        {
            ControlContext.Instance.SetState(talkController);
            
            talkController.StartTalk(npc);
        }
    }

    private void EnterNpcArea(Collider2D collision)
    {
        // 맞닿은 오브젝트가 NPC일 시
        if (collision.CompareTag("NPC"))
        {
            // 해당 NPC의 정보를 가져오기
            npc = collision.gameObject.GetComponent<Npc>();
            Debug.Log("keydown spacebar");
        }
    }

    private void ExitNpcArea(Collider2D collision)
    {
        // 맞닿은 오브젝트가 NPC일 시
        if (collision.CompareTag("NPC"))
        {
            // NPC의 정보를 초기화
            npc = null;
            Debug.Log("exit");
        }
    }

    /************************************************************
    * [공격키]
    * 
    * 마우스 방향으로 플레이어가 공격
    ************************************************************/

    private void OnAttackKeyPressed()
    {
        if (isStateIdle && Input.GetButtonDown("Attack"))
        {
            attackManager.OnAttack();
        }
    }

    /************************************************************
    * [메뉴키]
    * 
    * 메뉴창을 열음
    ************************************************************/

    private void OnMenuKeyPressed()
    {
        if (Input.GetButtonDown("Menu"))
        {
            playerVec = Vector2.zero;

            ControlContext.Instance.SetState(menuController);
            menuController.OpenMenu();
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
        if (playerState == PlayerState.Dashing)
        {
            MoveDash();
        }
        // 방향키(혹은 비슷한 장치) 이동에 따른 이동
        else if (playerState == PlayerState.Idle)
        {
            MoveCharacter();
        }
    }

    private void MoveDash()
    {
        // 캐릭터 및 서브 오브젝트 이동
        rigid.MovePosition(Vector2.Lerp(mainEntity.position, dashVec, player.DashSpeed * Time.deltaTime));
        subEntity.position = Vector2.Lerp(subEntity.position, dashVec, player.DashSpeed * Time.deltaTime);

        // 만약 두 오브젝트 사이가 일정 수준까지 가까워지면 이동제한 해제
        if (Vector2.Distance(dashVec, subEntity.position) <= stopDistance)
        {
            playerState = PlayerState.Idle;
            isRunning = true;

            Debug.Log("ready");
        }
    }

    private void MoveCharacter()
    {
        float speed = (isRunning) ? player.RunSpeed : player.MoveSpeed;
        rigid.velocity = playerVec.normalized * speed * Time.deltaTime;

        // 달리기를 멈추면 걷기로 전환
        if(CheckRunning(rigid.velocity) == false)
        {
            playerState = PlayerState.Idle;
        }
    }

    private bool CheckRunning(Vector2 vec)
    {
        return isRunning
            && vec != Vector2.zero;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnterNpcArea(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        ExitNpcArea(collision);
    }
}