using Assets.Script.System;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

namespace Assets.Script.UI
{
    public enum menuIcon
    {
        option, save, load,
        title, call, message
    }

    public class MenuUI : MonoBehaviour
    {
        public Blink blink;

        public GameObject menu;
        public GameObject cursor;

        public GameObject portraitMode;
        public GameObject landscapeMode;

        public GameObject icon;
        public GameObject optionIcon;
        public GameObject saveIcon;
        public GameObject loadIcon;
        public GameObject titleIcon;
        public GameObject callIcon;
        public GameObject msgIcon;

        public GameObject wifiOnIcon;
        public GameObject wifiOffIcon;
        public GameObject serviceIcon;
        public GameObject noServiceIcon;

        public GameObject battery;

        public TextMeshProUGUI timeText;

        private bool isWifiOn = false;
        private bool isSpreadOn = false;
        private bool timer = false;

        private float accumTime = 0;

        private string nowTime = "";

        void Awake()
        {
            // 옵션 메뉴 숨기기
            menu.gameObject.SetActive(false);
        }

        private void Update()
        {
            if(timer) accumTime += Time.deltaTime;
        }

        protected internal void setMenuView(bool isView)
        {
            menu.gameObject.SetActive(isView);
        }

        /************************************************************
        * [커서 이동]
        * 
        * 아이콘을 가리키는 커서 이동
        ************************************************************/

        protected internal void moveToOption() { setCursor(optionIcon); }
        protected internal void moveToSave() { setCursor(saveIcon); }
        protected internal void moveToLoad() { setCursor(loadIcon); }
        protected internal void moveToTitle() { setCursor(titleIcon); }
        protected internal void moveToCall() { setCursor(callIcon); }
        protected internal void moveToMsg() { setCursor(msgIcon); }

        private void setCursor(GameObject select)
        {
            if(cursor.transform.localPosition != select.transform.localPosition)
            {
                cursor.transform.localPosition = select.transform.localPosition;
                blink.resetA();
            }
            
        }

        /************************************************************
        * [기타 아이콘 및 설정 조작]
        * 
        * 위에 쓰이는 아이콘 외의 것들과 설정(시간, 베터리)을 조작
        ************************************************************/

        protected internal void setBackgroundMode(bool isPortrait)
        {
            if(isPortrait) // 세로 모드
            {
                portraitMode.SetActive(true);
                landscapeMode.SetActive(false);

                icon.SetActive(true);
            }
            else // 가로 모드
            {
                portraitMode.SetActive(false);
                landscapeMode.SetActive(true);

                icon.SetActive(false);
            }
        }

        protected internal void changeScreen(bool isPortrait)
        {
            if (isPortrait) // 세로 모드
            {
                portraitMode.SetActive(true);
                landscapeMode.SetActive(false);

                screenRotation(-90f, 2f);

                setAllIcon(true);
            }
            else // 가로 모드
            {
                portraitMode.SetActive(false);
                landscapeMode.SetActive(true);

                screenRotation(90f, 2f);

                setAllIcon(false);
            }
        }

        private void screenRotation(float rotate, float sec)
        {
            // 반복 시간 증가
        }


        protected internal void setAllIcon(bool isView)
        {
            if(isView) // 모든 아이콘(전파, 와이파이, 바탕화면 아이콘 등) 생성
            {
                icon.SetActive(true);
                battery.SetActive(true);
                setWiFi(isWifiOn);
                setService(isSpreadOn);
                timeText.text = nowTime;
            }
            else // 모든 아이콘(전파, 와이파이, 바탕화면 아이콘 등) 제거
            {
                icon.SetActive(false);
                battery.SetActive(false);
                wifiOnIcon.SetActive(false);
                wifiOffIcon.SetActive(false);
                serviceIcon.SetActive(false);
                noServiceIcon.SetActive(false);
                timeText.text = "";
            }
        }

        protected internal void setWiFi(bool isHaving)
        {
            if(isHaving) // 와이파이가 터지게 설정
            {
                wifiOffIcon.SetActive(false);
                wifiOnIcon.SetActive(true);
            }
            else // 와이파이가 안 터지게 설정
            {
                wifiOnIcon.SetActive(false);
                wifiOffIcon.SetActive(true);
            }

            isWifiOn = isHaving;
        }

        protected internal void setService(bool isService)
        {
            if(isService) // 휴대폰 신호가 터지게 설정
            {
                serviceIcon.SetActive(true);
                noServiceIcon.SetActive(false);
            }
            else // 휴대폰 신호가 안 터지게 설정
            {
                serviceIcon.SetActive(false);
                noServiceIcon.SetActive(true);
            }

            isSpreadOn = isService;
        }

        protected internal void setTime(int hour, int minute)
        {
            string time = "";

            if (hour >= 12) time = "PM ";
            else time = "AM ";

            time += hour + ":" + minute;
            timeText.text = time;

            nowTime = timeText.text;
        }

        public void UiReset()
        {
            icon.SetActive(false);
            battery.SetActive(false);
            wifiOnIcon.SetActive(false);
            wifiOffIcon.SetActive(false);
            serviceIcon.SetActive(false);
            noServiceIcon.SetActive(false);
            timeText.text = "";
        }
    }
}