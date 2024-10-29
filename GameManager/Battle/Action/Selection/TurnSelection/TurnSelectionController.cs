using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class TurnSelectionController : MonoBehaviour, IControlState
{
    [Header("참조 스크립트")]
    [SerializeField] private TurnSelection selection;

    private bool isMoveKeyPressed;

    public void OnConnected()
    {
        MainInput.BattleActions input = ControlContext.Instance.KeyInput.Battle;

        input.Navigate.performed += OnNavigateKeyPressed;
        input.Select.performed += OnSelectKeyPressed;
    }

    public void OnDisconnected()
    {
        MainInput.BattleActions input = ControlContext.Instance.KeyInput.Battle;

        input.Navigate.performed -= OnNavigateKeyPressed;
        input.Select.performed -= OnSelectKeyPressed;
    }

    public void OnNavigateKeyPressed(InputAction.CallbackContext context)
    {
        Vector2 moveInput = context.ReadValue<Vector2>();

        // 키보드 좌우키로 삽입할 타임라인 선택
        if (moveInput.x > 0) selection.MoveNext();
        else if (moveInput.x < 0) selection.MovePrev();
    }

    public void OnTimelineMoveKeyPressed()
    {
        float h = 0;

        if (h != 0 && !isMoveKeyPressed)
        {
            if (h > 0)
            {
                selection.MoveNext();
            }
            else if (h < 0)
            {
                selection.MovePrev();
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

    public void OnSelectKeyPressed(InputAction.CallbackContext context)
    {
        // 현재 칸에 타임라인 삽입
        selection.InsertAction();
    }
}