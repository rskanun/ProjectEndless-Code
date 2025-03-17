using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class SurveyController : MonoBehaviour, IController
{
    [Header("참조 스크립트")]
    [SerializeField] private SurveyManager manager;

    private bool isMoveKeyPressed;

    public void ControlConnect()
    {
        MainInput.UIActions input = ControlContext.Instance.KeyInput.UI;

        input.Navigate.performed += OnNavigateKeyPressed;
    }

    public void ControlDisconnect()
    {
        MainInput.UIActions input = ControlContext.Instance.KeyInput.UI;

        input.Navigate.performed -= OnNavigateKeyPressed;
    }

    private void OnNavigateKeyPressed(InputAction.CallbackContext context)
    {
        Vector2 moveInput = context.ReadValue<Vector2>();

        if (moveInput.x > 0) manager.SurveyNext();
        else if (moveInput.x < 0) manager.SurveyPrev();
    }
}