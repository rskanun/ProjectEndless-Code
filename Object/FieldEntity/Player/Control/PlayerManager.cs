using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private static PlayerManager _currentPlayer;
    public static PlayerManager CurrentPlayer => _currentPlayer;

    [SerializeField] private InteractManager interactManager;

    [Header("이동속도")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float runSpeed;

    // 플레이어 구성 컴포넌트
    private Rigidbody2D rigid;
    private Animator playerAnimator;

    // 이동 제어 변수
    private Vector2 direction;
    private bool isRunKeyPressed;
    private bool isRunning;

#if UNITY_EDITOR
    private void OnValidate()
    {
        rigid = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
    }
#endif

    private void OnEnable()
    {
        _currentPlayer = this;
    }

    private void OnDisable()
    {
        _currentPlayer = null;
    }

    public void HideCharacter()
    {
        // 몬스터 인카운트 시, 플레이어 오브젝트 숨기기
        gameObject.SetActive(false);
    }

    public void ShowCharacter()
    {
        // 필드로 돌아오면 다시 플레이어 오브젝트 보이기
        gameObject.SetActive(true);
    }

    /************************************************************
     * [상호작용]
     * 
     * 플레이어가 바라보는 대상과 상호작용 제어
     ************************************************************/

    public void Interact()
    {
        interactManager.OnInteract();
    }

    /************************************************************
     * [이동]
     * 
     * 플레이어의 이동을 제어
     ************************************************************/

    public void Teleport(Vector2 pos)
    {
        // 플레이어의 위치 변경
        transform.position = pos;

        // 인게임 데이터에도 변화 주기
        GameData.Instance.Position = transform.position;
    }

    /// <summary>
    /// 인게임 데이터를 기준으로 플레이어의 위치 다시 불러오기
    /// </summary>
    public void UpdateLocation()
    {
        transform.position = GameData.Instance.Position;
    }

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
        // 좌우 또는 앞뒤만 입력받은 경우 시야 돌리기
        if (direction.x == 0 ^ direction.y == 0)
        {
            interactManager.RotateEyes(direction);
        }
    }

    private void FixedUpdate()
    {
        // 걷기 체크
        isRunning = IsCanRunning(direction);

        // 현재 플레이어가 누른 방향으로 이동
        float speed = isRunning ? runSpeed : moveSpeed;
        rigid.velocity = direction.normalized * speed * Time.deltaTime;

        // 플레이어 위치 데이터 갱신
        GameData.Instance.Position = transform.position;
    }

    private bool IsCanRunning(Vector2 moveVec)
    {
        float absX = Mathf.Abs(moveVec.x);
        float absY = Mathf.Abs(moveVec.y);

        bool isWalkAxis = absX <= 0.5f && absY <= 0.5f;

        // 달리기 키가 눌러져 있거나 조이스틱 기울기가 뛰는 정도일 경우 뛰는 걸로 판정
        return isRunKeyPressed || !isWalkAxis;
    }
}