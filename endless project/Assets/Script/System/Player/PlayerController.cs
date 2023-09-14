using Assets.Script.Control.Text;
using Assets.Script.System;
using Assets.Script.System.Player;
using Assets.Script.UI.ObjectAnimation.Player;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // 플레이어의 마우스 위치에 따른 시선 변경 보정 상수
    private const int ANIMATION_CONSTANT = 10;
    
    // 대쉬 제동 거리
    private const float STOP_DISTANCE = 0.05f;

    // 플레이어와 대쉬 보조용 가상 엔티티 벡터
    private Vector2 playerVec;
    private Vector2 dashVec;

    [SerializeField] private Rigidbody2D rigid;
    [SerializeField] private Transform mainEntity; // 플레이어 캐릭터
    [SerializeField] private Transform subEntity; // 대쉬 예상 지점 계산을 위한 가상의 엔티티

    [Header("플레이어 데이터")]
    [SerializeField] private PlayerData player;

    [Header("참조 스크립트")]
    [SerializeField] private CharacterAnimation anim;
    [SerializeField] private TalkManager talkManager;
    [SerializeField] private AttackManager atkManager;

    // 참조 스크립터블 오브젝트
    private OptionSetting option;
    private PlayerState playerState;

    private void Start()
    {
        option = OptionSetting.Instance;
        playerState = PlayerState.Instance;
    }

    private void Update()
    {
        // 움직임 키 감지
        if (playerState.IsPlayerControllable)
        {
            moveKeyPress(); // 이동키
            dashKeyPress(); // 대쉬키
            attackKeyPress(); // 공격키
            talkingKeyPress(); // 대화키
        }
    }


    /************************************************************
     * [방향키]
     * 
     * wasd로 플레이어를 이동
     ************************************************************/

    private void moveKeyPress()
    {
        // 패드 및 키보드의 움직임(패드의 경우 경도)에 따른 백터 변화
        playerVec.x = Input.GetAxisRaw("Horizontal");
        playerVec.y = Input.GetAxisRaw("Vertical");

        // 키보드 누른 방향으로 애니메이션 움직임 제어
        player.Angle = playerVec;
    }

    /************************************************************
    * [대쉬키]
    * 
    * 마우스 방향으로 플레이어가 빠르게 이동
    ************************************************************/

    private void dashKeyPress()
    {
        if (Input.GetKeyDown(option.Dash))
        {
            // 대쉬 이동을 위한 계산식
            getDashVector();

            // 움직임 멈춤
            rigid.velocity = Vector2.zero;

            // 대쉬 작동
            playerState.IsDashing = true;
        }
    }

    private void getDashVector()
    {
        // 서브 엔티티를 플레이어로 이동
        subEntity.position = mainEntity.position;

        // 대쉬 벡터 초기화
        dashVec = Vector2.zero;

        // 캐릭터와 마우스 각도 계산
        Vector2 locClick = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 locChr = mainEntity.position;

        float angle = Mathf.Atan2(locClick.y - locChr.y, locClick.x - locChr.x)
            * Mathf.Rad2Deg;

        // 이동 거리 계산
        float distance = player.Speed;

        dashVec.x = Mathf.Cos(angle * Mathf.Deg2Rad) * distance;
        dashVec.y = Mathf.Sin(angle * Mathf.Deg2Rad) * distance;

        // 대쉬 애니메이션 설정
        setDashAnimation();

        // 현재 좌표값을 기준으로 대쉬 좌표 재설정
        dashVec.x += locChr.x;
        dashVec.y += locChr.y;
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

    /************************************************************
    * [대화키]
    * 
    * 대사를 읽어 그에 따른 인게임 이벤트 제어
    ************************************************************/

    public void talkingKeyPress()
    {
        // 대화가 처음이고 가능한 상태일 경우
        if (playerState.IsPlayerControllable)
        {
            // 대화가능한 npc가 범위 내에 있다면 상호작용 키로 대화를 활성화
            if (player.Npc is not null && Input.GetKeyDown(option.Interact))
            {
                talkManager.initTalk(player.Npc);
            }
        }
    }

    /************************************************************
    * [공격키]
    * 
    * 마우스 방향으로 플레이어가 공격
    ************************************************************/

    private void attackKeyPress()
    {
        // 일반 공격
        if(Input.GetKeyDown(option.Attack) && playerState.IsAttacking == false)
        {
            atkManager.OnNormalAttack();
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
        if(playerState.IsPlayerControllable)
        {
            moveCharacter();
        }
    }

    private void moveDash()
    {
        // 캐릭터 및 서브 오브젝트 이동
        rigid.MovePosition(Vector2.Lerp(mainEntity.position, dashVec, player.DashSpeed));
        subEntity.position = Vector2.Lerp(subEntity.position, dashVec, player.DashSpeed);

        // 만약 두 오브젝트 사이가 일정 수준까지 가까워지면 이동제한 해제
        if (Vector2.Distance(dashVec, subEntity.position) <= STOP_DISTANCE)
        {
            playerState.IsDashing = false;

            Debug.Log("ready");
        }
    }

    private void moveCharacter()
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(playerState.IsAttacking == false)
        {
            // 맞닿은 오브젝트가 NPC일 시
            if (collision.CompareTag(Tag.NPC))
            {
                // 해당 NPC의 정보를 가져오기
                player.Npc = collision.gameObject.GetComponent<NPC>();
                Debug.Log("keydown " + option.Interact.ToString());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (playerState.IsAttacking == false)
        {    
            // 맞닿은 오브젝트가 NPC일 시
            if (collision.CompareTag(Tag.NPC))
            {
                // NPC의 정보를 초기화
                player.Npc = null;
                Debug.Log("exit");
            }
        }
    }
}