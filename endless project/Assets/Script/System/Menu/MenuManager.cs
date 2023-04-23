using Assets.Script.Control;
using Assets.Script.System.Interface.Menu.App;
using Assets.Script.UI;
using System.Collections;
using UnityEditor;
using UnityEngine;

namespace Assets.Script.System.Menu
{
    public class MenuManager : MonoBehaviour
    {
        private MenuUI ui;
        private NoKeyDown noKeyDown;

        // 메뉴 앱 관련 변수
        private App nowApp = null;
        private bool isAppClose = true;
        public bool IsAppEmpty
        {
            get { return nowApp == null && isAppClose == true; }
        }
        public bool IsMenuControlable
        {
            get { return IsAppEmpty && noKeyDown.AllowMenuKey; }
        }

        private static MenuManager _instance;
        public static MenuManager Instance { get { return _instance; } }

        private void Awake()
        {
            _instance = this;
        }

        private void Start()
        {
            noKeyDown = NoKeyDown.Instance;
            ui = MenuUI.Instance;
        }

        public void menuOpen()
        {
            noKeyDown.IsMenuActive = true;
            ui.menuOpen();
        }

        public void menuClose()
        {
            noKeyDown.IsMenuActive = false;
            ui.menuClose();
        }

        /************************************************************
        * [앱 제어]
        * 
        * 메뉴에 존재하는 앱들을 제어
        ************************************************************/

        public void appOpen(App app)
        {
            nowApp = app;
            nowApp.open();

            isAppClose = false;
        }

        public void appClose()
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

        public void closeAllApps()
        {
            while (isAppClose == false)
            {
                appClose();
            }
        }

    }
}