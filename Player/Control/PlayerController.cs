using UnityEngine;
using UnityEngine.Playables;

public class PlayerController : MonoBehaviour, IControlState
{
    // 현재 플레이어 캐릭터 상태
    private bool isRunning = false;

    // 플레이어 입력 키 벡터
    private Vector2 arrowKeyVec;

    // 현재 상호작용 가능한 NPC
    private Npc npc;

    [Header("이동속도")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float runSpeed;

    [Header("관련 오브젝트")]
    [SerializeField] private Rigidbody2D rigid;
    [SerializeField] private PlayerData player;

    [Header("참조 스크립트")]
    [SerializeField] private PlayerAnimation playerAnima;
    [SerializeField] private TalkController talkController;
    [SerializeField] private MenuController menuController;

    private void Start()
    {
        ControlContext.Instance.SetState(this);

        transform.position = player.Position;
    }

    public void OnControlKeyPressed()
    {
        OnMoveKeyPressed();
        OnRunKeyPressed();
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
        Vector2 vec = Vector2.zero;

        // 패드 및 키보드의 움직임(패드의 경우 경도)에 따른 백터 변화
        vec.x = Input.GetAxisRaw("Horizontal");
        vec.y = Input.GetAxisRaw("Vertical");

        arrowKeyVec = vec;

        // 걷는 정도의 스피드인지 판단
        CheckingWalk(arrowKeyVec.x, arrowKeyVec.y);

        // 키보드 누른 방향으로 애니메이션 움직임 제어
        playerAnima.SetPlayerAngleAnim(arrowKeyVec);
    }

    private void CheckingWalk(float x, float y)
    {
        float absX = Mathf.Abs(x);
        float absY = Mathf.Abs(y);

        bool isWalkSpeed = absX <= 0.5f && absY <= 0.5f;

        // 움직임 정도가 일정 이하면 걷기
        if (!isRunning && isWalkSpeed)
        {
            isRunning = false;
        }
        else if (!isRunning)
        {
            isRunning = true;
        }
    }

    /************************************************************
     * [달리기 키]
     * 
     * 플레이어의 달리기를 제어
     ************************************************************/

    private void OnRunKeyPressed()
    {
        if(isRunning == false && Input.GetButtonDown("Running"))
        {
            isRunning = true;
        }
    }

    /************************************************************
    * [대화키]
    * 
    * 바라보는 대상과 대화 시작
    ************************************************************/

    private void OnTalkKeyPressed()
    {
        if(npc != null && Input.GetButtonDown("Talking"))
        {
            arrowKeyVec = Vector2.zero;

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
    * [메뉴키]
    * 
    * 메뉴창을 열음
    ************************************************************/

    private void OnMenuKeyPressed()
    {
        if (Input.GetButtonDown("Menu"))
        {
            arrowKeyVec = Vector2.zero;

            ControlContext.Instance.SetState(menuController);
            menuController.OpenMenu();
        }
    }

    /************************************************************
    * [물리 시스템]
    * 
    * 실제 게임 내의 캐릭터의 행동에 따른 변화
    ************************************************************/

    private void Update()
    {
        player.Position = transform.position;
    }

    private void FixedUpdate()
    {
        float speed = (isRunning) ? runSpeed : moveSpeed;
        rigid.velocity = arrowKeyVec.normalized * speed * Time.deltaTime;

        // 달리기를 멈추면 걷기로 전환
        if (CheckRunning(rigid.velocity) == false)
        {
            isRunning = false;
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