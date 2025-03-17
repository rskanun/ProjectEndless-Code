using System;
using DG.Tweening;
using UnityEngine;

public abstract class AppUI : MonoBehaviour
{
    [SerializeField] protected GameObject window;

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

    public void OpenApp(bool isPlayAnimation)
    {
        homeScreenUI.DisabledHomeScreen(isPlayAnimation);
        AppOpenAnimation(isPlayAnimation);
    }

    protected abstract Sequence AppOpenAnimation(bool isPlayAnimation);

    public void CloseApp(bool isPlayAnimation)
    {
        homeScreenUI.EnabledHomeScreen(isPlayAnimation);
        AppCloseAnimation(isPlayAnimation);
    }

    protected abstract Sequence AppCloseAnimation(bool isPlayAnimation);
}