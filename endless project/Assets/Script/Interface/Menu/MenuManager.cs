using Assets.Script.Control;
using Assets.Script.Interface.Menu.App;
using Assets.Script.UI;
using UnityEngine;

namespace Assets.Script.System.Menu
{
    public class MenuManager : MonoBehaviour
    {
        [Header("메뉴 및 앱 리스트")]
        public HomeScreen homeScreen;
        public App optionApp;
        public Messanger messangerApp;

        [Header("참조 스크립트")]
        public MenuControl menuCtr;
        public MenuUI ui;

        /************************************************************
        * [아이콘]
        * 
        * 메뉴의 각 아이콘의 기능 수행
        ************************************************************/

        public void iconSelect(UI.MenuApp icon)
        {
            switch(icon)
            {
                case UI.MenuApp.Option:
                    option();
                    break;

                case UI.MenuApp.Save:
                    save();
                    break;

                case UI.MenuApp.Load:
                    load();
                    break;

                case UI.MenuApp.Title:
                    title();
                    break;

                case UI.MenuApp.Call:
                    call();
                    break;

                case UI.MenuApp.Message:
                    homeScreen.appOpen(messangerApp);
                    break;

                default:
                    break;
            }
        }

        public void option()
        {
            homeScreen.appOpen(optionApp);
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
            homeScreen.appOpen(messangerApp);
        }
    }
}