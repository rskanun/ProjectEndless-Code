using UnityEngine;

public abstract class AppUI : MonoBehaviour
{
    [Header("참조 오브젝트")]
    [SerializeField] protected GameObject appBackground;
    [SerializeField] protected GameObject homeScreen;

    [Header("참조 스크립트")]
    [SerializeField] private HomeScreenUI homeScreenUI;

    /************************************************************
    * [앱 애니메이션]
    * 
    * 애니메이션 조작 관리
    ************************************************************/

    public void OpenApp(GameObject window)
    {
        homeScreenUI.DisabledHomeScreen();
        AppOpenAnimation(window);
    }

    protected abstract void AppOpenAnimation(GameObject window);

    public void CloseApp(GameObject window)
    {
        homeScreenUI.EnabledHomeScreen();
        AppCloseAnimation(window);
    }

    protected abstract void AppCloseAnimation(GameObject window);
}