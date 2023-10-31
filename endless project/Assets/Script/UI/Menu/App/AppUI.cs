using TMPro;
using UnityEditor.PackageManager.UI;
using UnityEngine;

namespace Assets.Script.UI.Menu
{
    public class AppUI : MonoBehaviour
    {
        public GameObject appBackground;
        public GameObject homeScreen;
        [Header("참조 스크립트")]
        [SerializeField] private HomeScreenUI homeScreenUI;

        /************************************************************
        * [앱 애니메이션]
        * 
        * 애니메이션 조작 관리
        ************************************************************/

        public void openApp(GameObject window)
        {
            homeScreenUI.setAllAppButton(false);
            appOpenAnimation(window);
        }

        protected virtual void appOpenAnimation(GameObject window)
        {
            AppAnimation.hideHomeScreenAnimation(homeScreen);
            AppAnimation.openAppAnimation(window, appBackground, homeScreen);
        }

        public void closeApp(GameObject window)
        {
            homeScreenUI.setAllAppButton(true);
            appCloseAnimation(window);
        }

        protected virtual void appCloseAnimation(GameObject window)
        {
            AppAnimation.showHomeScreenAnimation(homeScreen);
            AppAnimation.closeAppAnimation(window, appBackground, homeScreen);
        }
    }
}