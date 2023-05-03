using TMPro;
using UnityEngine;

namespace Assets.Script.UI.Menu
{
    public class AppUI : MonoBehaviour
    {
        [SerializeField] private GameObject appBackground;
        [SerializeField] private GameObject homeScreen;
        [Header("취소 패널")]
        [SerializeField] private GameObject cancelPanel;
        [Header("앱 버튼")]
        [SerializeField] private GameObject optionButton;
        [SerializeField] private GameObject saveButton;
        [SerializeField] private GameObject loadButton;
        [SerializeField] private GameObject titleButton;
        [SerializeField] private GameObject callButton;
        [SerializeField] private GameObject messageButton;

        public void setCancelPanel(bool isVeiw)
        {
            cancelPanel.SetActive(isVeiw);
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