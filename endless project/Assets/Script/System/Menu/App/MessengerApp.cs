using Assets.Script.Control;
using Assets.Script.System.Menu;
using Assets.Script.System.Option;
using Assets.Script.UI;
using Assets.Script.UI.Menu;
using System.Collections;
using UnityEngine;
using UnityEngine.Windows;

namespace Assets.Script.System.Menu.App
{
    public class MessengerApp : App
    {
        [SerializeField] private ToastUI toast;

        private PhoneOptionSetting setting;
        private Coroutine networkChecking;

        private void Start()
        {
            setting = PhoneOptionSetting.Instance;
        }

        public override void open()
        {
            base.open();

            // 앱 열린 후 체킹
            if(networkChecking != null)
                StopCoroutine(networkChecking);

            networkChecking = StartCoroutine(checkingNetwork());
        }

        IEnumerator checkingNetwork()
        {
            // 로딩 시간
            yield return new WaitForSeconds(1.0f);

            if(setting.Network == false)
            {
                Alert.makeMsg("네트워크 상태가 원활하지 않습니다. 네트워크를 연결한 후 다시 접속해주세요.")
                .setOkCallBack(() =>
                {
                    MenuManager.Instance.appClose();
                }).show();
            }
            else openMainScreen();

            networkChecking = null;
        }

        private void openMainScreen()
        {
            // 홈 화면 출력
        }

        public override bool close()
        {
            if(networkChecking != null)
                StopCoroutine(networkChecking);

            toast.setActive(false);

            return base.close();
        }
    }
}