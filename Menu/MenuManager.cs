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
            .OnComplete(() => control.KeyUnlock());
    }

    public void CloseMenu()
    {
        // 메뉴가 닫히는 동안 키 입력 무시
        control.KeyLock();

        // 메뉴 닫기 애니메이션
        ui.CloseMenu()
            .OnComplete(() =>
            {
                // 열려있는 앱이 있을 경우
                if (currentApp != null)
                {
                    // 열려있는 모든 앱 닫기
                    CloseAllApps(false);
                }

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

        app.Open(true);
    }

    public void CloseApp(bool isPlayAnimation = true)
    {
        if (currentApp != null)
        {
            currentApp.Close(isPlayAnimation);

            if (!currentApp.IsActive)
            {
                currentApp = null;
            }
        }
    }

    public void CloseAllApps(bool isPlayAnimation = true)
    {
        int count = 0;
        while (currentApp != null)
        {
            if (count > 100)
            {
                Debug.LogWarning("앱 화면 끄기 -> 내부에서 종료로 변경");
                return;
            }

            CloseApp(isPlayAnimation);
            count++;
        }
    }
}