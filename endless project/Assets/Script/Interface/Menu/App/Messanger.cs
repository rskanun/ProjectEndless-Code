using Assets.Script.UI;
using System.Collections;
using UnityEngine;

namespace Assets.Script.Interface.Menu.App
{
    public class Messanger : App
    {
        public MenuUI ui;

        [Space]
        [Header("경고창 오브젝트")]
        public GameObject alert;

        private Coroutine wifiCheck;

        public override void open()
        {
            appAnimation.openSimpleAppAnimation(window);

            if(wifiCheck != null)
                StopCoroutine(wifiCheck);

            wifiCheck = StartCoroutine(checkingWifi());
        }

        IEnumerator checkingWifi()
        {
            // 로딩 시간
            yield return new WaitForSeconds(1f);

            WaitForSeconds wait = new WaitForSeconds(0.5f);

            while(ui.IsWifiActive == false)
            {
                if (!alert.activeSelf)
                    appAnimation.alertOnAnimation(alert);

                // 홈화면 열기

                yield return wait;
            }

            appAnimation.alertOffAnimation(alert);
            wifiCheck = null;
        }

        public override bool close()
        {
            if(alert.activeSelf) 
                alert.SetActive(false);
            
            if(wifiCheck != null)
                StopCoroutine(wifiCheck);

            return base.close();
        }
    }
}