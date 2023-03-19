using Assets.Script.UI;
using System.Collections;
using UnityEngine;

namespace Assets.Script.Interface.Menu.App
{
    public class Messanger : App
    {
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
                Debug.Log("1");
                ui.alert("ㅆㅃ");

                yield return wait;
            }

            networkChecking = null;

            openMainScreen();
        }

        private void openMainScreen()
        {
            // 홈 화면 출력
        }

        public override bool close()
        {   
            if(networkChecking != null)
                StopCoroutine(networkChecking);

            return base.close();
        }
    }
}