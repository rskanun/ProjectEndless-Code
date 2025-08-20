using System;
using DG.Tweening;
public class MessengerUI : AppUI
{
    public override void OpenApp(Action openHandler)
    {
        MenuAnimation.AppOpenAnimation(window, appBackground)
            .AppendCallback(() => openHandler?.Invoke());
    }

    public override void CloseApp(Action closeHandler)
    {
        MenuAnimation.AppCloseAnimation(window, appBackground)
            .AppendCallback(() => closeHandler?.Invoke());
    }
}