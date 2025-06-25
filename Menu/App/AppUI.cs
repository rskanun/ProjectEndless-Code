using System;
using DG.Tweening;
using UnityEngine;

public abstract class AppUI : MonoBehaviour
{
    /************************************************************
    * [앱 애니메이션]
    * 
    * 애니메이션 조작 관리
    ************************************************************/

    public void OpenApp(bool isPlayAnimation)
    {
        AppOpenAnimation(isPlayAnimation);
    }

    protected abstract Sequence AppOpenAnimation(bool isPlayAnimation);

    public void CloseApp(bool isPlayAnimation)
    {
        AppCloseAnimation(isPlayAnimation);
    }

    protected abstract Sequence AppCloseAnimation(bool isPlayAnimation);
}