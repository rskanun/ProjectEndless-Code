using DG.Tweening;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [Header("참조 스크립트")]
    [SerializeField] private MenuUI ui;

    private ControlContext control;

    // 앱 상태
    private App currentApp;
    public bool IsOpenedApp => currentApp != null;
    public bool IsOpenedDiary { get; private set; }

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
        // 플레이어 컨트롤러 비활성화
        control.DisableController(typeof(PlayerController));

        // 메뉴가 열리는 동안 키 입력 무시
        control.KeyLock();

        // 메뉴 열기 애니메이션
        ui.OpenMenu()
            .AppendCallback(() => control.KeyUnlock());
    }

    public void CloseMenu()
    {
        // 메뉴가 닫히는 동안 키 입력 무시
        control.KeyLock();

        // 다이어리가 열려있으면 닫기
        if (IsOpenedDiary) CloseDiary();

        // 메뉴 닫기 애니메이션
        ui.CloseMenu()
            .AppendCallback(() =>
            {
                // 열려있는 앱 완전 종료시키기
                ShutdownApp();

                // 키 입력 활성화
                control.KeyUnlock();

                // 플레이어 컨트롤러 활성화
                control.EnableController(typeof(PlayerController));
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

        // 홈 화면 숨기기
        ui.DisabledHomeScreen();

        // 앱 열기
        app.Open();
    }

    public void CloseApp()
    {
        // 현재 열린 앱이 있다면 앱 종료
        if (currentApp != null)
        {
            currentApp.Close();

            // 앱이 종료되었을 경우 홈 화면 불러오기
            if (!currentApp.IsActive)
            {
                currentApp = null;
                ui.EnabledHomeScreen();
            }
        }
    }

    public void ShutdownApp()
    {
        // 앱 완전 종료 시키기
        currentApp?.Shutdown();
        currentApp = null;
    }

    /************************************************************
    * [다이어리 제어]
    * 
    * 메뉴의 부가적인 창인 다이어리 열고 닫기를 제어
    ************************************************************/

    public void OpenDiary()
    {
        IsOpenedDiary = true;

        ui.OpenDiary();
    }

    public void CloseDiary()
    {
        IsOpenedDiary = false;

        ui.CloseDiary();
    }
}