using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class PlayerController : MonoBehaviour, IController
{
    [Header("참조 스크립트")]
    [SerializeField] private PlayerManager player;
    [SerializeField] private InteractManager interactManager;
    [SerializeField] private MenuManager menuManager;

    // 컨트롤러
    private MainInput.PlayerActions input;

    private void Awake()
    {
        input = ControlContext.Instance.KeyInput.Player;
    }

    private void Start()
    {
        ControlContext context = ControlContext.Instance;

        // 플레이어 컨트롤러를 초기값으로 설정
        context.SetInitController(this);
        context.SetController(this);

        // transform.position = player.Position;
    }

    public void OnConnected()
    {
        input.Enable();

        input.Movement.performed += OnMoveKeyPressed;
        input.Movement.canceled += OnMoveKeyPressed;
        input.Running.performed += OnRunKeyPressed;
        input.Running.canceled += OnRunKeyPressed;
        input.Menu.performed += OnMenuKeyPressed;
    }

    public void OnDisconnected()
    {
        input.Disable();

        input.Movement.performed -= OnMoveKeyPressed;
        input.Movement.canceled -= OnMoveKeyPressed;
        input.Running.performed -= OnRunKeyPressed;
        input.Running.canceled -= OnRunKeyPressed;
        input.Menu.performed -= OnMenuKeyPressed;
    }

    /************************************************************
     * [이동키]
     * 
     * 플레이어의 이동을 제어
     ************************************************************/

    private void OnMoveKeyPressed(InputAction.CallbackContext context)
    {
        // 패드 및 키보드의 움직임(패드의 경우 경도)에 따른 백터 변화
        Vector2 direction = input.Movement.ReadValue<Vector2>();

        // 해당 벡터로 플레이어 움직이기
        player.MoveTo(direction);
    }

    private void OnRunKeyPressed(InputAction.CallbackContext context)
    {
        player.SetRunning(input.Running.WasPressedThisFrame());
    }

    /************************************************************
    * [메뉴키]
    * 
    * 메뉴창을 열기
    ************************************************************/

    private void OnMenuKeyPressed(InputAction.CallbackContext context)
    {
        menuManager.OpenMenu();
    }
}