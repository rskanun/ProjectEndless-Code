using Assets.Script.Control;
using Assets.Script.Control.Interface.Menu.App;
using Assets.Script.UI;
using UnityEngine;

namespace Assets.Script.System.Menu
{
    public class MenuManager : MonoBehaviour
    {
        [Header("메뉴 및 앱 리스트")]
        public HomeScreen homeScreen;
        public App optionApp;

        [Header("참조 스크립트")]
        public MenuControl menuCtr;
        public MenuUI ui;

        public void setSelectPos(MenuIcon icon)
        {
            int iconNum = (int)icon;

            menuCtr.setSelectPos(iconNum);
            moveSelectTo(icon);
        }

        public void moveSelectTo(MenuIcon icon)
        {
            ui.setCursorPos((int)icon);
        }

        /************************************************************
        * [아이콘]
        * 
        * 메뉴의 각 아이콘의 기능 수행
        ************************************************************/

        public void iconSelect(MenuIcon icon)
        {
            switch(icon)
            {
                case MenuIcon.Option:
                    homeScreen.appOpen(optionApp);
                    break;

                case MenuIcon.Save:
                    save();
                    break;

                case MenuIcon.Load:
                    load();
                    break;

                case MenuIcon.Title:
                    title();
                    break;

                case MenuIcon.Call:
                    call();
                    break;

                case MenuIcon.Message:
                    message();
                    break;

                default:
                    break;
            }
        }

        public void save()
        {
            Debug.Log("save");
        }

        public void load()
        {
            Debug.Log("load");
        }

        public void title()
        {
            Debug.Log("title");
        }

        public void call()
        {
            Debug.Log("call");
        }

        public void message()
        {
            Debug.Log("message");
        }
    }
}