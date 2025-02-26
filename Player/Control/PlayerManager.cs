using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerManager : MonoBehaviour
{
    // 이동 제어 변수
    private Vector2 direction;
    private bool isRunKeyPressed;
    private bool isRunning;

    [Header("참조 스크립트")]
    [SerializeField] private InteractManager interactManager;
    [SerializeField] private TalkManager talkManager;

    [Header("플레이어 구성 요소")]
    [SerializeField] private Rigidbody2D rigid;
    [SerializeField] private Animator playerAnimator;

    [Header("이동속도")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float runSpeed;

    private void Awake()
    {
        // 현재 플레이어가 있는 구역을 카메라 이동 범위로 등록
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, new Vector2(0.01f, 0.01f), 0);
        foreach (Collider2D collider in colliders)
        {
            Debug.Log(collider.name);
            if (collider is TilemapCollider2D tilemap && collider.CompareTag("Area"))
            {
                MapManager.SetCurrentArea(tilemap);
                break;
            }
        }
    }

    /************************************************************
     * [이동]
     * 
     * 플레이어의 이동을 제어
     ************************************************************/

    public void MoveTo(Vector2 direction)
    {
        this.direction = direction;

        // 움직임에 따른 애니메이션 제어
        SetPlayerMoveAnim(direction);
        SetSightDirection(direction);
    }

    public void SetRunning(bool isRunning)
    {
        // 달리기 키 상태 변경
        isRunKeyPressed = isRunning;

        // 달리기 키를 눌렀다면 달리는 상태로 변경
        if (isRunKeyPressed)
        {
            this.isRunning = true;
        }
    }

    private void SetPlayerMoveAnim(Vector2 direction)
    {
        int h = (int)direction.x;
        int v = (int)direction.y;

        if (h == 0 || v == 0)
        {
            playerAnimator.SetInteger("axisH", h);
            playerAnimator.SetInteger("axisV", v);
        }
    }

    private void SetSightDirection(Vector2 direction)
    {
        if (direction.x == 0 ^ direction.y == 0)
        {
            interactManager.RotateEyes(direction);
        }
    }

    private void FixedUpdate()
    {
        // 걷기 체크
        CheckingWalk(direction);

        float speed = isRunning ? runSpeed : moveSpeed;
        rigid.velocity = direction.normalized * speed * Time.deltaTime;
    }

    private void CheckingWalk(Vector2 moveVec)
    {
        float absX = Mathf.Abs(moveVec.x);
        float absY = Mathf.Abs(moveVec.y);

        bool isWalkAxis = absX <= 0.5f && absY <= 0.5f;

        // 달리기 키가 눌려져 있지 않은 상태에서 조이스틱 기울기가 걷는 정도일 경우 달리기 종료
        if (!isRunKeyPressed && isWalkAxis)
        {
            isRunning = false;
        }
    }

    /************************************************************
     * [상호작용]
     * 
     * 플레이어와의 상호작용 제어
     ************************************************************/

    public void OnTalking(Npc npc)
    {
        talkManager.StartTalk(npc);
    }
}