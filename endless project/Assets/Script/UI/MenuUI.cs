using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.UI
{
    public enum menuIcon
    {
        option, save, load,
        title, call, message
    }

    public class MenuUI : MonoBehaviour
    {
        public GameObject menu;
        public GameObject cursor;

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

        public Text timeText;

        void Awake()
        {
            // 옵션 메뉴 숨기기
            menu.gameObject.SetActive(false);
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
            cursor.transform.localPosition = select.transform.localPosition;
        }

        /************************************************************
        * [기타 아이콘 및 설정 조작]
        * 
        * 위에 쓰이는 아이콘 외의 것들과 설정(시간, 베터리)을 조작
        ************************************************************/

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
        }

        protected internal void setTime(int hour, int minute)
        {
            string time = "";

            if (hour >= 12) time = "PM ";
            else time = "AM ";

            time += hour + ":" + minute;
            timeText.text = time;
        }
    }
}