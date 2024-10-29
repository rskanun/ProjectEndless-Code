using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class SurveyController : MonoBehaviour, IControlState
{
    [Header("참조 스크립트")]
    [SerializeField] private SurveyManager manager;

    private bool isMoveKeyPressed;

    public void OnConnected()
    {
        MainInput.BattleActions input = ControlContext.Instance.KeyInput.Battle;

        input.Navigate.performed += OnNavigateKeyPressed;
    }

    public void OnDisconnected()
    {
        MainInput.BattleActions input = ControlContext.Instance.KeyInput.Battle;

        input.Navigate.performed -= OnNavigateKeyPressed;
    }

    public void OnNavigateKeyPressed(InputAction.CallbackContext context)
    {
        Vector2 moveInput = context.ReadValue<Vector2>();

        if (moveInput.x > 0) manager.SurveyNext();
        else if (moveInput.x < 0) manager.SurveyPrev();
    }

    public void OnControlKeyPressed()
    {
        OnTimelineMoveKeyPressed();
    }

    public void OnTimelineMoveKeyPressed()
    {
        float h = 0; //Input.GetAxisRaw(KeyOption.AxisH);

        if (h != 0 && !isMoveKeyPressed)
        {
            if (h > 0)
            {
                manager.SurveyNext();
            }
            else if (h < 0)
            {
                manager.SurveyPrev();
            }

            // 움직였으면 다음 움직임은 키에서 손을 땔 때까지 막기
            isMoveKeyPressed = true;
        }
        else if (h == 0)
        {
            // 키에서 손을 땠다면 다음 키를 누를 수 있도록 변경
            isMoveKeyPressed = false;
        }
    }
}