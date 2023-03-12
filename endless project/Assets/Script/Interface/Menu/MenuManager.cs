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

        /************************************************************
        * [아이콘]
        * 
        * 메뉴의 각 아이콘의 기능 수행
        ************************************************************/

        public void iconSelect(MenuApp icon)
        {
            switch(icon)
            {
                case MenuApp.Option:
                    option();
                    break;

                case MenuApp.Save:
                    save();
                    break;

                case MenuApp.Load:
                    load();
                    break;

                case MenuApp.Title:
                    title();
                    break;

                case MenuApp.Call:
                    call();
                    break;

                case MenuApp.Message:
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