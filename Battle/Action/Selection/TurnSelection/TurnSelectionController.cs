using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class TurnSelectionController : MonoBehaviour, IController
{
    [Header("참조 스크립트")]
    [SerializeField] private TurnSelection selection;

    private bool isMoveKeyPressed;

    public void ControlConnect()
    {
        MainInput.UIActions input = ControlContext.Instance.KeyInput.UI;

        input.Navigate.performed += OnNavigateKeyPressed;
        input.Select.performed += OnSelectKeyPressed;
    }

    public void ControlDisconnect()
    {
        MainInput.UIActions input = ControlContext.Instance.KeyInput.UI;

        input.Navigate.performed -= OnNavigateKeyPressed;
        input.Select.performed -= OnSelectKeyPressed;
    }

    private void OnNavigateKeyPressed(InputAction.CallbackContext context)
    {
        Vector2 moveInput = context.ReadValue<Vector2>();

        // 키보드 좌우키로 삽입할 타임라인 선택
        if (moveInput.x > 0) selection.MoveNext();
        else if (moveInput.x < 0) selection.MovePrev();
    }

    private void OnSelectKeyPressed(InputAction.CallbackContext context)
    {
        // 현재 칸에 타임라인 삽입
        selection.InsertAction();
    }
}