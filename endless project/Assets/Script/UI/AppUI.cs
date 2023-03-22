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
        [Header("앱 버튼")]
        public GameObject optionButton;
        public GameObject saveButton;
        public GameObject loadButton;
        public GameObject titleButton;
        public GameObject callButton;
        public GameObject messageButton;

        public void alert(string msg)
        {
            if(alertMsg.activeSelf == true)
                alertMsg.SetActive(false);

            alertTxt.text = msg;
            AppAnimation.alertAnimation(alertMsg, 0.15f, 1.5f);
        }

        public void alertStop()
        {
            if(alertMsg.activeSelf == true)
            {
                alertMsg.SetActive(false);
            }
        }

        /************************************************************
        * [앱 애니메이션]
        * 
        * 애니메이션 조작 관리
        ************************************************************/

        public void openApp(GameObject window)
        {
            setAllAppButton(false);
            AppAnimation.hideHomeScreenAnimation(homeScreen);
            AppAnimation.openAppAnimation(window, appBackground, homeScreen);
        }

        public void closeApp(GameObject window)
        {
            setAllAppButton(true);
            AppAnimation.showHomeScreenAnimation(homeScreen);
            AppAnimation.closeAppAnimation(window, appBackground, homeScreen);
        }

        public void openAppSimple(GameObject window)
        {
            setAllAppButton(false);
            AppAnimation.hideHomeScreenAnimation(homeScreen);
            AppAnimation.openSimpleAppAnimation(window, appBackground, homeScreen);
        }

        private void setAllAppButton(bool isActive)
        {
            optionButton.SetActive(isActive);
            saveButton.SetActive(isActive);
            loadButton.SetActive(isActive);
            titleButton.SetActive(isActive);
            callButton.SetActive(isActive);
            messageButton.SetActive(isActive);
        }
    }
}