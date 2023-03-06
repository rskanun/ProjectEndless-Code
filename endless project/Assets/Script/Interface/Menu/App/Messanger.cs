using Assets.Script.UI;
using System.Collections;
using UnityEngine;

namespace Assets.Script.Interface.Menu.App
{
    public class Messanger : App
    {
        [Space]
        [Header("경고창 오브젝트")]
        public GameObject alert;

        private PhoneOptionSetting setting;
        private Coroutine networkChecking;

        private void Start()
        {
            setting = PhoneOptionSetting.Instance;
        }

        public override void open()
        {
            ui.openAppSimple(window);

            if(networkChecking != null)
                StopCoroutine(networkChecking);

            networkChecking = StartCoroutine(checkingNetwork());
        }

        IEnumerator checkingNetwork()
        {
            // 로딩 시간
            yield return new WaitForSeconds(1f);

            WaitForSeconds wait = new WaitForSeconds(0.5f);

            while(setting.Network == false)
            {
                if (!alert.activeSelf)
                    ui.alertOn(alert);

                openHomeScreen();

                yield return wait;
            }

            ui.alertOff(alert);
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