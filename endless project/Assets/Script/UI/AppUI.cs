using TMPro;
using UnityEngine;

namespace Assets.Script.UI
{
    public class AppUI : MonoBehaviour
    {
        public GameObject appBackground;
        public GameObject homeScreen;
        [Header("알림창")]
        public GameObject alertMsg;
        public TextMeshProUGUI alertTxt;

        public void alert(string msg)
        {
            alertTxt.text = msg;
            AppAnimation.alertOnAnimation(alertMsg);
        }    

        /************************************************************
        * [앱 애니메이션]
        * 
        * 애니메이션 조작 관리
        ************************************************************/

        public void openApp(GameObject window)
        {
            homeScreen.SetActive(false);
            AppAnimation.openAppAnimation(window, appBackground, homeScreen);
        }

        public void closeApp(GameObject window)
        {
            homeScreen.SetActive(true);
            AppAnimation.closeAppAnimation(window, appBackground, homeScreen);
        }

        public void openAppSimple(GameObject window)
        {
            homeScreen.SetActive(false);
            AppAnimation.openSimpleAppAnimation(window, appBackground, homeScreen);
        }
    }
}