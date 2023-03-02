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

        private Coroutine networkChecking;

        public override void open()
        {
            appAnimation.openSimpleAppAnimation(window);

            if(networkChecking != null)
                StopCoroutine(networkChecking);

            networkChecking = StartCoroutine(checkingNetwork());
        }

        IEnumerator checkingNetwork()
        {
            // 로딩 시간
            yield return new WaitForSeconds(1f);

            WaitForSeconds wait = new WaitForSeconds(0.5f);

            while(ui.IsNetworkActive == false)
            {
                if (!alert.activeSelf)
                    appAnimation.alertOnAnimation(alert);

                openHomeScreen();

                yield return wait;
            }

            appAnimation.alertOffAnimation(alert);
            networkChecking = null;
        }

        private void openHomeScreen()
        {
            // 홈 화면 출력
        }

        public override bool close()
        {
            if(alert.activeSelf) 
                alert.SetActive(false);
            
            if(networkChecking != null)
                StopCoroutine(networkChecking);

            return base.close();
        }
    }
}