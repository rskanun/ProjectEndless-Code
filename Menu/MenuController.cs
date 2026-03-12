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

        uiInput.Menu.performed += OnMenuKeyPressed;
        uiInput.Cancel.performed += OnCancelKeyPressed;
    }

    public void ControlDisconnect()
    {
        MainInput.UIActions uiInput = ControlContext.Instance.KeyInput.UI;

        uiInput.Menu.performed -= OnMenuKeyPressed;
        uiInput.Cancel.performed -= OnCancelKeyPressed;
    }

    private void OnMenuKeyPressed(InputAction.CallbackContext context)
    {
        // 현재 메뉴가 열려있고, 입력장치가 키보드인 경우
        if (isOpened && context.control.device is Keyboard)
        {
            // 뒤로가기 키와 겹치는 것을 방지해 메뉴 닫기의 경우 실행 X
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
        // 메뉴가 열린 상태에서만 동작
        if (!isOpened) return;

        // 우선순위에 따라 팝업 -> 앱 -> 메뉴 순서로 닫고 함수 종료
        if (PopupManager.Instance.isActive)
        {
            PopupManager.Instance.Close();
            return;
        }

        if (menuManager.IsOpenedApp)
        {
            menuManager.CloseApp();
            return;
        }

        // 어떠한 팝업이나 앱도 열려있지 않다면 메뉴 닫기
        menuManager.CloseMenu();
        isOpened = false;
    }
}