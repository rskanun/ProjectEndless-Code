using UnityEngine;
using UnityEngine.InputSystem;

public class MenuController : MonoBehaviour, IController
{
    [Header("참조 스크립트")]
    [SerializeField]
    private MenuManager menuManager;

    // 메뉴 상태
    private bool isOpened;

    private void Awake()
    {
        ControlContext.Instance.EnableController(this);
    }

    private void OnDestroy()
    {
        ControlContext.Instance.RemoveController(this);
    }

    public void ControlConnect()
    {
        MainInput.UIActions uiInput = ControlContext.Instance.KeyInput.UI;

        uiInput.Cancel.performed += OnCancelKeyPressed;
        uiInput.Menu.performed += OnMenuKeyPressed;
    }

    public void ControlDisconnect()
    {
        MainInput.UIActions uiInput = ControlContext.Instance.KeyInput.UI;

        uiInput.Cancel.performed -= OnCancelKeyPressed;
        uiInput.Menu.performed -= OnMenuKeyPressed;
    }

    private void OnMenuKeyPressed(InputAction.CallbackContext context)
    {
        if (menuManager.IsOpenedApp)
        {
            // 앱이 열린 상태면 무시
            return;
        }

        // 현재 메뉴 상태에 따라 열기 or 닫기
        if (isOpened) menuManager.CloseMenu();
        else menuManager.OpenMenu();

        // 메뉴 상태 변경
        isOpened = !isOpened;
    }

    private void OnCancelKeyPressed(InputAction.CallbackContext context)
    {
        if (!isOpened || PopupManager.Instance.isActive)
        {
            // 메뉴가 열린 상태가 아니거나 팝업 창이 열려있다면 무시
            return;
        }

        // 앱 닫기
        if (menuManager.IsOpenedApp)
        {
            // 앱이 열린 상태에서만 작동
            menuManager.CloseApp();
        }
    }
}