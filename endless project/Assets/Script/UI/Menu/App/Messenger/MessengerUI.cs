using System.Collections;
using UnityEngine;

namespace Assets.Script.UI.Menu.App.Messenger
{
    public class MessengerUI : AppUI
    {
        protected override void appCloseAnimation(GameObject window)
        {
            AppAnimation.hideHomeScreenAnimation(homeScreen);
            AppAnimation.openSimpleAppAnimation(window, appBackground, homeScreen);
        }
    }
}