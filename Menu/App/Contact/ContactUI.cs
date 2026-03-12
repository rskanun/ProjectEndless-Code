using System;
using DG.Tweening;
using UnityEngine;

public class ContactUI : AppUI
{
    [Header("참조 오브젝트")]
    [SerializeField] private MenuManager menu;

    public override void OpenApp(Action openHandler)
    {
        DOTween.Sequence()
            .Join(MenuAnimation.AppOpenAnimation(window, appBackground))
            .JoinCallback(menu.OpenDiary)
            .AppendCallback(() => openHandler?.Invoke());
    }

    public override void CloseApp(Action closeHandler)
    {
        DOTween.Sequence()
            .Join(MenuAnimation.AppCloseAnimation(window, appBackground))
            .JoinCallback(menu.CloseDiary)
            .AppendCallback(() => closeHandler?.Invoke());
    }
}