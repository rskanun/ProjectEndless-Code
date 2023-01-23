using Assets.Script.System;
using System.Collections;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.Image;

namespace Assets.Script.UI
{
    public enum MenuIcon
    {
        Option, Save, Load,
        Title, Call, Message
    }

    public class MenuUI : MonoBehaviour
    {
        public GameObject cursor;
        [Space]

        [Header("아이콘")]
        public GameObject optionIcon;
        public GameObject saveIcon;
        public GameObject loadIcon;
        public GameObject titleIcon;
        public GameObject callIcon;
        public GameObject msgIcon;
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
        [Space]

        [Header("애니메이션")]
        public GameObject gameManager;
        public GameObject appAnimationImage;

        private CustomAnimation cusAnimation;

        private void Awake()
        {
            cusAnimation = gameManager.AddComponent<CustomAnimation>();
        }

        /************************************************************
        * [커서 이동]
        * 
        * 아이콘을 가리키는 커서 이동
        ************************************************************/

        public void moveToOption() { setCursor(optionIcon); }
        public void moveToSave() { setCursor(saveIcon); }
        public void moveToLoad() { setCursor(loadIcon); }
        public void moveToTitle() { setCursor(titleIcon); }
        public void moveToCall() { setCursor(callIcon); }
        public void moveToMsg() { setCursor(msgIcon); }

        private void setCursor(GameObject select)
        {
            if(cursor.transform.localPosition != select.transform.localPosition)
            {
                cursor.transform.localPosition = select.transform.localPosition;
                blink.resetA();
            }
            
        }

        /************************************************************
        * [애니메이션]
        * 
        * 다른 창으로 넘어갈 때 실행되는 애니메이션 관리
        ************************************************************/

        public IEnumerator openAppAnimation(GameObject window)
        {
            Vector3 originScale = appAnimationImage.transform.localScale;
            Vector2 originLocation = window.transform.localPosition;
            window.transform.localPosition = new Vector2(originLocation.x, originLocation.y - window.GetComponent<RectTransform>().rect.height);

            Debug.Log(originLocation);
            Debug.Log(window.transform.localPosition);

            // animation
            appAnimationImage.SetActive(true);
            yield return StartCoroutine(cusAnimation.bigger(appAnimationImage, 10, 0.08f, 40));
            yield return new WaitForSeconds(0.1f);
            yield return StartCoroutine(cusAnimation.moveTo(window, originLocation, 0.2f, 40));
            Debug.Log(window.transform.localPosition);

            // destroy
            appAnimationImage.transform.localScale = originScale;
            appAnimationImage.SetActive(false);
        }

        public IEnumerator openWindowAnimation(GameObject window)
        {
            yield return null;
        }

        public void closeApp()
        {

        }

        public void toLandscapeMode()
        {

        }

        public void toPortraitMode()
        {

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

            if (hour >= 12) time = "PM " + (hour - 12);
            else time = "AM " + hour;

            time += ":" + minute;
            timeText.text = time;
        }
    }
}