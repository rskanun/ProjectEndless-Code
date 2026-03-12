using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IController
{
    [Header("참조 스크립트")]
    [SerializeField] private PlayerManager player;
    [SerializeField] private InteractManager interactManager;

    public void OnEnable()
    {
        ControlContext.Instance.EnableController(this);
    }

    private void OnDisable()
    {
        ControlContext.Instance.DisableController(this);
    }

    private void OnDestroy()
    {
        ControlContext.Instance.RemoveController(this);
    }

    public void ControlConnect()
    {
        MainInput.PlayerActions input = ControlContext.Instance.KeyInput.Player;

        input.Movement.performed += OnMoveKeyPressed;
        input.Movement.canceled += OnMoveKeyPressed;
        input.Running.performed += OnRunKeyPressed;
        input.Running.canceled += OnRunKeyPressed;
        input.Interact.performed += OnInteractKeyPressed;
    }

    public void ControlDisconnect()
    {
        MainInput.PlayerActions input = ControlContext.Instance.KeyInput.Player;

        input.Movement.performed -= OnMoveKeyPressed;
        input.Movement.canceled -= OnMoveKeyPressed;
        input.Running.performed -= OnRunKeyPressed;
        input.Running.canceled -= OnRunKeyPressed;
        input.Interact.performed -= OnInteractKeyPressed;
    }

    /************************************************************
     * [이동키]
     * 
     * 플레이어의 이동을 제어
     ************************************************************/

    private void OnMoveKeyPressed(InputAction.CallbackContext context)
    {
        // 패드 및 키보드의 움직임(패드의 경우 경도)에 따른 백터 변화
        Vector2 direction = context.ReadValue<Vector2>();

        // 해당 벡터로 플레이어 움직이기
        player.MoveTo(direction);
    }

    private void OnRunKeyPressed(InputAction.CallbackContext context)
    {
        bool isPressed = context.ReadValue<float>() > 0;

        player.SetRunning(isPressed);
    }

    /************************************************************
     * [상호작용키]
     * 
     * 플레이어의 상호작용 제어
     ************************************************************/

    private void OnInteractKeyPressed(InputAction.CallbackContext context)
    {
        // 이동 방향을 (0,0)으로 설정
        player.MoveTo(Vector2.zero);

        // 상호작용 시작
        player.Interact();
    }
}