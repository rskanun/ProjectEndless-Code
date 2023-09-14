

public class PlayerState
{
    private static PlayerState _instance;
    public static PlayerState Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new PlayerState();
            }
            
            return _instance;
        }
    }

    /************************************************************
    * [플레이어 상태]
    * 
    * 현제 플레이어의 상태에 관한 변수
    ************************************************************/

    // 플레이어가 현재 대시를 하고 있는 상태인지 여부
    private bool _isDashing = false;
    public bool IsDashing
    {
        get { return _isDashing; }
        set { _isDashing = value; }
    }

    // 현재 플레이어가 NPC와 대화를 진행중인 상태인지 여부
    private bool _isTalking = false;
    public bool IsTalking
    {
        get { return _isTalking; }
        set { _isTalking = value; }
    }

    // 메뉴 화면이 현재 켜져있는지 여부
    private bool _isMenuActive = false;
    public bool IsMenuActive
    {
        get { return _isMenuActive; }
        set { _isMenuActive = value; }
    }

    // 현제 플레이어가 뛰고있는 상태인지 여부
    private bool _isRunning = false;
    public bool IsRunning
    {
        get { return _isRunning; }
        set { _isRunning = value; }
    }

    // 공격하는 도중인지 여부
    private bool _isAttacking = false;
    public bool IsAttacking
    {
        get { return _isAttacking; }
        set { _isAttacking = value; }
    }

    /************************************************************
    * [행동 제약]
    * 
    * 플레이어 행동 제약에 관한 변수
    ************************************************************/

    // 플레이어를 조종할 수 있는지 여부
    private bool _isPlayerControllable = true;
    public bool IsPlayerControllable
    {
        get
        {
            if (_isPlayerControllable)
                return _isDashing == false && _isTalking == false && _isMenuActive == false && _isAttacking == false;
            else
                return _isPlayerControllable;
        }

        set { _isPlayerControllable = value; }
    }

    // 메뉴키를 누를 수 있는지 여부
    private bool _allowMenuKey = true;
    public bool AllowMenuKey
    {
        get
        {
            if (_allowMenuKey)
                return _isTalking == false;
            else
                return false;
        }

        set { _allowMenuKey = value; }
    }

    // 뒤로가기 키를 누를 수 있는지 여부
    private bool _allowCancelKey = false;
    public bool AllowCancelKey
    {
        get
        {
            if (_isMenuActive)
                return _allowMenuKey;

            return _allowCancelKey;
        }

        set { _allowCancelKey = value; }
    }
}