using Assets.Script.UI;
using UnityEngine;

namespace Assets.Script.Interface.Menu.App
{
    public class HomeScreen : App
    {
        private const float CLOSE_ROTATE = 70, OPEN_ROTATE = 0;
        public bool playAnimation
        {
            get
            {
                Quaternion rotate = window.transform.rotation;
                return rotate.z != 0 && rotate.z != 70;
            }
        }

        // 앱 관련 변수
        private App nowApp = null;
        private bool isAppClose = true;

        public bool isAppEmpty
        {
            get { return nowApp == null && isAppClose == true; }
        }

        public override void open()
        {
            ui.openMenu(window, OPEN_ROTATE, CLOSE_ROTATE);
        }

        public void appOpen(App app)
        {
            nowApp = app;
            nowApp.open();

            isAppClose = false;
        }

        public void cancel()
        {
            if (isAppClose == false)
            {
                isAppClose = nowApp.close();

                if (isAppClose)
                {
                    nowApp = null;
                }
            }
        }

        public override bool close()
        {
            ui.closeMenu(window, OPEN_ROTATE, CLOSE_ROTATE);

            return true;
        }

        public void closeAll()
        {
            while(isAppClose == false)
            {
                cancel();
            }
        }
    }
}