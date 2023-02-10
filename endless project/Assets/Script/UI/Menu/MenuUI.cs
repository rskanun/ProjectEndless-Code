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
    public enum MenuIcon
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
        public GameObject cursor;

        [Space]
        [Header("아이콘")]
        public List<GameObject> icon = new List<GameObject>();

        [Space]
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

        [Space]
        [Header("스크립트")]
        public Blink blink;

        private void Start()
        {
            // init phone UI;
            setService(true);
            setWiFi(true);

            setTime(15, 29);
        }

        /************************************************************
        * [커서 이동]
        * 
        * 아이콘을 가리키는 커서 이동
        ************************************************************/

        public void setCursorPos(int index)
        {
            GameObject select = icon[index];

            if (cursor.transform.localPosition != select.transform.localPosition)
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

        public void setWiFi(bool isHaving)
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

        public void setService(bool isService)
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

        public void setTime(int hour, int minute)
        {
            string time;

            if (hour >= 12) time = "PM " + (hour - 12);
            else time = "AM " + hour;

            time += ":" + minute;
            timeText.text = time;
        }
    }
}