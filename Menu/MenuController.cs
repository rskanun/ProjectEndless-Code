using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuController : MonoBehaviour, IController
{
    [Header("참조 스크립트")]
    [SerializeField]
    private MenuManager menuManager;
    private PopupManager popupManager;

    private MainInput.MenuActions menuInput;
    private MainInput.UIActions uiInput;

    private void Awake()
    {
        menuInput = ControlContext.Instance.KeyInput.Menu;
        uiInput = ControlContext.Instance.KeyInput.UI;
    }

    private void Start()
    {
        popupManager = PopupManager.Instance;
    }

    public void OnConnected()
    {
        menuInput.Enable();
        uiInput.Enable();

        menuInput.Menu.performed += OnMenuKeyPressed;
        uiInput.Cancel.performed += OnCancelKeyPressed;
    }

    public void OnDisconnected()
    {
        menuInput.Disable();
        uiInput.Disable();

        menuInput.Menu.performed -= OnMenuKeyPressed;
        uiInput.Cancel.performed -= OnCancelKeyPressed;
    }

    private void OnMenuKeyPressed(InputAction.CallbackContext context)
    {
        // 메뉴 닫기
        menuManager.CloseMenu();
    }

    private void OnCancelKeyPressed(InputAction.CallbackContext context)
    {
        // 메뉴 혹은 앱 닫기
        if (!popupManager.isActive)
        {
            // 앱이 열려있다면 앱부터 삭제
            if (menuManager.IsOpenedApp) menuManager.CloseApp();
            else menuManager.CloseMenu();
        }
    }
}