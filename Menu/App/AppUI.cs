using System;
using UnityEngine;

public abstract class AppUI : MonoBehaviour
{
    [SerializeField] protected HomeScreenUI homeScreenUI;
    [SerializeField] protected GameObject window;
    [SerializeField] protected GameObject appBackground;

    protected bool _isOpened;
    public bool IsOpened => _isOpened;

    /************************************************************
    * [앱 애니메이션]
    * 
    * 애니메이션 조작 관리
    ************************************************************/

    public void OpenApp(bool isPlayAnimation, Action openHandler)
    {
        homeScreenUI.DisabledHomeScreen(isPlayAnimation);

        if (!isPlayAnimation) // 애니메이션 없이 앱 열기
            ActiveApp(openHandler);
        else // 애니메이션이 진행된 뒤에 앱 열기
            ActiveAppWithAnimation(openHandler);
    }

    /// <summary>
    /// 애니메이션을 실행시키지 않고 앱 활성화
    /// </summary>
    protected virtual void ActiveApp(Action openHandler)
    {
        appBackground.SetActive(true);
        window.SetActive(true);

        // 활성화가 전부 진행된 뒤에 핸들러 진행
        openHandler?.Invoke();

        _isOpened = true;
    }

    /// <summary>
    /// 앱을 활성화시킬 애니메이션
    /// </summary>
    /// <param name="openHandler">앱 활성화 이후 실행될 함수</param>
    protected abstract void ActiveAppWithAnimation(Action openHandler);

    public void CloseApp(bool isPlayAnimation)
    {
        homeScreenUI.EnabledHomeScreen(isPlayAnimation);

        if (!isPlayAnimation) // 애니메이션 없이 앱 닫기
            DeactiveApp();
        else // 애니메이션이 진행된 뒤에 앱 닫기
            DeactiveAppWithAnimation();
    }

    protected virtual void DeactiveApp()
    {
        _isOpened = false;

        appBackground.SetActive(false);
        window.SetActive(false);
    }

    /// <summary>
    /// 앱을 비활성화시킬 애니메이션
    /// </summary>
    protected abstract void DeactiveAppWithAnimation();
}