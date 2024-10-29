using DG.Tweening;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [Header("참조 스크립트")]
    [SerializeField] private MenuUI ui;
    [SerializeField] private MenuController menuController;

    private ControlContext control;
    private PopupManager popupManager;

    // 현재 열려있는 앱
    private App currentApp;
    public bool IsOpenedApp => currentApp != null;

    private void Awake()
    {
        control = ControlContext.Instance;
    }

    /************************************************************
    * [메뉴 제어]
    * 
    * 메뉴의 열고 닫기를 제어
    ************************************************************/

    public void OpenMenu()
    {
        // 컨트롤러 변경
        control.SetState(menuController);

        // 메뉴가 열리는 동안 키 입력 무시
        control.KeyLock();

        // 메뉴 열기 애니메이션
        ui.OpenMenu()
            .OnComplete(() => control.KeyUnlock());
    }

    public void CloseMenu()
    {
        // 메뉴가 닫히는 동안 키 입력 무시
        control.KeyLock();

        // 열려있는 앱이 있을 경우
        if (currentApp != null)
        {
            // 열려있는 모든 앱 닫기
            CloseAllApps();
        }

        // 메뉴 닫기 애니메이션
        ui.CloseMenu()
            .OnComplete(() =>
            {
                control.KeyUnlock();

                // 메뉴가 닫히면 초기 컨트롤러로 변경
                control.ResetState();
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

            if (!currentApp.IsActive)
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