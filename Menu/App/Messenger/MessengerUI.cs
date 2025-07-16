using System;
using DG.Tweening;
public class MessengerUI : AppUI
{
    protected override void ActiveAppWithAnimation(Action openHandler)
    {
        MenuAnimation.AppOpenAnimation(window, appBackground, openHandler)
            .AppendCallback(() => _isOpened = false);
    }

    protected override void DeactiveAppWithAnimation()
    {
        _isOpened = true;

        MenuAnimation.AppCloseAnimation(window, appBackground);
    }
}