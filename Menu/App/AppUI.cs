using System;
using UnityEngine;

public abstract class AppUI : MonoBehaviour
{
    [SerializeField] protected GameObject window;
    [SerializeField] protected GameObject appBackground;

    /************************************************************
    * [앱 애니메이션]
    * 
    * 애니메이션 조작 관리
    ************************************************************/

    /// <summary>
    /// 애니메이션을 이용해 앱 열기
    /// </summary>
    /// <param name="openHandler">앱 활성화 이후 실행될 함수</param>
    public abstract void OpenApp(Action openHandler);

    /// <summary>
    /// 애니메이션을 이용해 앱 닫기
    /// </summary>
    public abstract void CloseApp(Action closeHandler);

    /// <summary>
    /// 애니메이션을 실행시키지 않고 앱 닫기
    /// </summary>
    public virtual void DeactiveApp()
    {
        appBackground.SetActive(false);
        window.SetActive(false);
    }
}