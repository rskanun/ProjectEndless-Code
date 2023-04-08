using Assets.Script.System.Option;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.UI
{
    public enum MenuApp
    {
        Option = 0,
        Save = 1,
        Load = 2,
        Title = 3,
        Call = 4,
        Message = 5
    }

    public class MenuUI : MonoBehaviour
    {
        [Space]
        [Header("메뉴")]
        [SerializeField] private GameObject menu;
        [SerializeField] private GameObject window;
        [SerializeField] private GameObject displayUI;

        [Header("WiFi")]
        [SerializeField] private GameObject wifiOnIcon;
        [SerializeField] private GameObject wifiOffIcon;

        [Space]
        [Header("전파")]
        [SerializeField] private GameObject serviceIcon;
        [SerializeField] private GameObject noServiceIcon;

        [Space]
        [SerializeField] private GameObject battery;
        [SerializeField] private TextMeshProUGUI timeText;

        private PhoneOptionSetting menuOption;
        private OptionSetting option;

        // 메뉴 열리고 닫히는 각도
        private const float CLOSE_ROTATE = 70, OPEN_ROTATE = 0;

        private void Start()
        {
            menuOption = PhoneOptionSetting.Instance;
            option = OptionSetting.Instance;

            // init phone UI;
            setService(menuOption.Service);
            setWiFi(menuOption.Network);
        }

        public void menuOpen()
        {
            timeUpdate();
            AppAnimation.openMenuAnimation(menu, window, displayUI, OPEN_ROTATE, CLOSE_ROTATE);
        }

        public void menuClose()
        {
            AppAnimation.closeMenuAnimation(menu, window, displayUI, OPEN_ROTATE, CLOSE_ROTATE);
        }

        /************************************************************
        * [기타 아이콘 및 설정 조작]
        * 
        * 위에 쓰이는 아이콘 외의 것들과 설정(시간, 베터리)을 조작
        ************************************************************/

        public void setWiFi(bool isHaving)
        {
            menuOption.Network = isHaving;

            wifiOffIcon.SetActive(!isHaving);
            wifiOnIcon.SetActive(isHaving);
        }

        public void setService(bool isService)
        {
            menuOption.Service = isService;

            serviceIcon.SetActive(isService);
            noServiceIcon.SetActive(!isService);
        }

        public void timeUpdate()
        {
            int hour = option.Hour;
            int min = option.Minute;

            string timeTxt = (hour < 12) ? "AM" : "PM";
            timeTxt += " ";
            timeTxt += (hour > 12) ? (hour - 12) : hour;
            timeTxt += ":";
            timeTxt += (min < 10) ? "0" + min : min;

            timeText.text = timeTxt;
        }
    }
}