using Assets.Script.System.Menu;
using System.Collections;
using UnityEngine;

public class Control : MonoBehaviour
{
    private const float STOP_DISTANCE = 0.05f;

    private const int ANIMATION_CONSTANT = 10;

    private Vector2 vec;
    private Vector2 dashVec;

    private Animator animator;

    private Rigidbody2D rigid;
    public GameObject subRigid;

    private Transform mainEntity; // 플레이어 캐릭터
    private Transform subEntity; // 대쉬 예상 지점 계산을 위한 가상의 엔티티

    // 참조 스크립트
    private EventManager command;
    private MenuManager menuManager;
    private TextManager text;
    private TextUI textUI;
    private NPC npc; // 상호작용 할 npc

    [SerializeField]
    private Player player;

    [SerializeField]
    private GameObject dialog;

    [SerializeField]
    private GameObject menuUI;

    // 캐릭터를 움직이는 모든 키 차단
    private bool noMoveKeyDown
    {
        get { return isTalking || isDashing; }
    }

    // 옵션(ESC) 키 차단
    private bool noOptionKeyDown
    {
        get { return isTalking; }
    }

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

    // 옵션키
    private string menu      = Option.getKey(Key.menu);

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
        command     = GetComponent<EventManager>();

        // UI Canvas -> Text Window
        text        = dialog.GetComponent<TextManager>();
        textUI      = dialog.GetComponent<TextUI>();

        // UI Canvas -> Menu
        menuManager = menuUI.GetComponent<MenuManager>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(menu))
        {
            menuManager.menuView();
        }

        // 텍스트 상호작용 키 감지
        // 대화가능한 npc가 범위 내에 있다면 상호작용 키로 대화를 활성화
        // 대화 도중 액션키와 상호작용 키만 인식
        if (!(npc is null) && Input.GetKeyDown(interact) || isTalking && Input.GetKeyDown(action))
        {
            talking();
        }

        // 움직임 키 감지
        if (!noMoveKeyDown)
        {
            moveKeyPress();
        }
    }

    /************************************************************
     * [움직임 제어]
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

    /************************************************************
    * [대쉬 키]
    * 
    * 마우스 방향으로 플레이어가 빠르게 이동
    ************************************************************/

    private bool isDashing = false;

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
    * [대화 출력]
    * 
    * 인게임 화면의 대화 제어
    ************************************************************/

    private int lineNum;
    private int lineCnt;

    private string[] lines;

    private float typingSpeed;

    private bool isTalking = false;

    public void talking()
    {
        // 첫 대화의 시작일 경우
        if (!isTalking)
        {
            // 현재 대사 번호 리셋
            lineNum = 0;

            // 대화 가능한 npc일 경우
            if(npc.getID() != 0)
            {
                // 대화 처음 시작 시 해당되는 대화목록 가져오기
                lines = text.getText(npc.getID());

                // 대화 진행상태로 변경
                isTalking = true;
            }
        }

        // 한 대사를 모두 출력시 그 대사 종료
        if (lineCnt >= lines[lineNum].Length)
        {
            lineCnt = 0;
            lineNum++;

            // 텍스트 창 비활성화
            textUI.setDialogView(false);
        }

        // 대화 진행
        if (lineNum < lines.Length)
        {
            // 대사 가져오기
            char[] line = lines[lineNum].ToCharArray();

            // 그 대사가 커맨드일 경우 이벤트 출력
            if (line[0] == '/')
            {
                command.getCommandEvent(lines[lineNum]);
                lineNum++;

                talking();
            }

            // 대사 출력
            else
                printText(line);
        }

        // 대화 종료
        else
        {
            // 텍스트 창 비활성화
            textUI.setDialogView(false);

            // 대화 종료상태로 변경
            isTalking = false;
        }
    }
    private void printText(char[] line)
    {
        // 한 글자도 출력이 안 됐을 경우
        if (lineCnt == 0)
        {
            // 텍스트 창 활성화 및 타이핑 속도 리셋
            textUI.setDialogView(true);
            typingSpeed = Option.getTypingSpeed();

            // 지정된 타이핑 속도로 출력
            StartCoroutine(talkDelay(line));
        }

        // 대화 출력 도중일 경우
        else if (lineCnt < line.Length)
        {
            // 한 번에 출력
            typingSpeed = 0;
        }
    }

    IEnumerator talkDelay(char[] line)
    {
        // 대화 진행 도중일 경우
        while (lineCnt < line.Length)
        {
            // 한 글자씩 대화를 출력
            textUI.setText(splitString(line, lineCnt++));

            yield return new WaitForSeconds(typingSpeed);
        }
    }

    // 길이만큼의 문자열 자르기
    private string splitString(char[] chrs, int length)
    {
        string result = "";

        for(int i = 0; i < length; i++)
        {
            result += chrs[i];
        }

        return result;
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
            Debug.Log("keydown " + interact);
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
        return Input.GetKey(up) || Input.GetKey(down)
            || Input.GetKey(left) || Input.GetKey(right);
    }
}