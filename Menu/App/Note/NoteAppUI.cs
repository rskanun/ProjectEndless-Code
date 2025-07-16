using System;
using DG.Tweening;

public class NoteAppUI : AppUI
{
    protected override void ActiveAppWithAnimation(Action openHandler)
    {
        MenuAnimation.AppOpenAnimation(window, appBackground, openHandler)
            .AppendCallback(() => _isOpened = true);
    }

    protected override void DeactiveAppWithAnimation()
    {
        _isOpened = false;

        MenuAnimation.AppCloseAnimation(window, appBackground);
    }
}