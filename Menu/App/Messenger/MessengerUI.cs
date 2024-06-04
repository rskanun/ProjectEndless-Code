using UnityEngine;

public class MessengerUI : AppUI
{
    protected override void AppCloseAnimation(GameObject window)
    {
        MenuAnimation.AppCloseAnimation(window, appBackground, homeScreen);
    }

    protected override void AppOpenAnimation(GameObject window)
    {
        MenuAnimation.AppOpenAnimation(window, appBackground, homeScreen);
    }
}