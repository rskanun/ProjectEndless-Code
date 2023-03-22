using Assets.Script.Interface.Menu;
using Assets.Script.System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEditor.Build.Content;
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
        public GameObject menu;

        [Header("WiFi")]
        public GameObject wifiOnIcon;
        public GameObject wifiOffIcon;

        [Space]
        [Header("전파")]
        public GameObject serviceIcon;
        public GameObject noServiceIcon;

        [Space]
        public GameObject battery;
        public TextMeshProUGUI timeText;

        private PhoneOptionSetting option;

        // 메뉴 열리고 닫히는 각도
        private const float CLOSE_ROTATE = 70, OPEN_ROTATE = 0;
        public bool IsPlayingAnimation
        {
            get
            {
                Quaternion rotate = menu.transform.rotation;
                return rotate.z != OPEN_ROTATE && rotate.z != CLOSE_ROTATE;
            }
        }

        private void Start()
        {
            option = PhoneOptionSetting.Instance;

            // init phone UI;
            setService(option.Service);
            setWiFi(option.Network);

            timeUpdate();
        }

        public void menuOpen()
        {
            AppAnimation.openMenuAnimation(menu, OPEN_ROTATE, CLOSE_ROTATE);
        }

        public void menuClose()
        {
            AppAnimation.closeMenuAnimation(menu, OPEN_ROTATE, CLOSE_ROTATE);
        }

        /************************************************************
        * [기타 아이콘 및 설정 조작]
        * 
        * 위에 쓰이는 아이콘 외의 것들과 설정(시간, 베터리)을 조작
        ************************************************************/

        public void setWiFi(bool isHaving)
        {
            option.Network = isHaving;

            wifiOffIcon.SetActive(!isHaving);
            wifiOnIcon.SetActive(isHaving);
        }

        public void setService(bool isService)
        {
            option.Service = isService;

            serviceIcon.SetActive(isService);
            noServiceIcon.SetActive(!isService);
        }

        public void timeUpdate()
        {
            timeText.text = option.Time;
        }
    }
}