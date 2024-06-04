using DG.Tweening;
using UnityEngine;

public class MenuController : MonoBehaviour, IControlState
{
    [Header("참조 스크립트")]
    [SerializeField]
    private PlayerController playerController; 
    private MenuUI ui;
    private App currentApp;
    private PopupManager popupManager;

    private bool keyPressBlock = false;

    private void Start()
    {
        ui = GetComponent<MenuUI>();
        popupManager = PopupManager.Instance;
    }

    public void OnControlKeyPressed()
    {
        OnCancelKeyPressed();
    }

    private void OnCancelKeyPressed()
    {
        if (Input.GetButtonDown("Cancel") && !keyPressBlock && !popupManager.isActive)
        {
            if (currentApp != null) CloseApp();
            else CloseMenu();
        }
    }

    /************************************************************
    * [메뉴 제어]
    * 
    * 메뉴의 열고 닫기를 제어
    ************************************************************/

    public void OpenMenu()
    {
        keyPressBlock = true;

        ui.OpenMenu()
            .OnComplete(() => keyPressBlock = false);
    }

    public void CloseMenu()
    {
        keyPressBlock = true;

        ui.CloseMenu()
            .OnComplete(() =>
            {
                keyPressBlock = false;

                ControlContext.Instance.SetState(playerController);
            });
    }

    /************************************************************
    * [앱 제어]
    * 
    * 메뉴에 존재하는 앱들을 제어
    ************************************************************/

    public void OpenApp(App app)
    {
        currentApp = app;

        app.Open();
    }

    public void CloseApp()
    {
        if (currentApp != null)
        {
            currentApp.Close();

            if (!currentApp.isActive)
            {
                currentApp = null;
            }
        }
    }

    public void CloseAllApps()
    {
        while (currentApp != null)
        {
            CloseApp();
        }
    }
}