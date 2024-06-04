using UnityEngine;

public class OptionUI : SubWindowUI
{
    protected override void AppCloseAnimation(GameObject window)
    {
        MenuAnimation.AppCloseAnimation(window, appBackground, homeScreen);
    }

    protected override void AppOpenAnimation(GameObject window)
    {
        MenuAnimation.AppToastOpenAnimation(window, appBackground, homeScreen);
    }
}