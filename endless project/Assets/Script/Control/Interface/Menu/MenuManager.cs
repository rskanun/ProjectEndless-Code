using Assets.Script.Control;
using Assets.Script.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script.System.Menu
{
    public class MenuManager : MonoBehaviour
    {
        public MenuUI ui;

        // 윈도우 오브젝트 리스트
        public GameObject optionWindow;
        public List<GameObject> subOptionWindows = new List<GameObject>();

        // 참조 스크립트
        private MenuControl menuCtr;

        // 참조 스크립트가 있는 오브젝트
        public GameObject gameManager;

        private void Awake()
        {
            // init component
            menuCtr = gameManager.GetComponent<MenuControl>();

            setPhoneUI();
        }

        private void setPhoneUI()
        {
            ui.setService(true);
            ui.setWiFi(true);

            ui.setTime(3, 29);
        }

        public void setSelectPos(MenuIcon icon)
        {
            int iconNum = (int)icon;

            menuCtr.setSelectPos(iconNum);
            moveSelectTo(icon);
        }

        public void moveSelectTo(MenuIcon icon)
        {
            switch(icon)
            {
                case MenuIcon.Option:   ui.moveToOption(); break;
                case MenuIcon.Save:     ui.moveToSave(); break;
                case MenuIcon.Load:     ui.moveToLoad(); break;
                case MenuIcon.Title:    ui.moveToTitle(); break;
                case MenuIcon.Call:     ui.moveToCall(); break;
                case MenuIcon.Message:  ui.moveToMsg(); break;
                default: break;
            }
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
                case MenuIcon.Option:   option(); break;
                case MenuIcon.Save:     save(); break;
                case MenuIcon.Load:     load(); break;
                case MenuIcon.Title:    title(); break;
                case MenuIcon.Call:     call(); break;
                case MenuIcon.Message:  message(); break;
                default: break;
            }
        }

        public void option()
        {
            menuCtr.openWindow(optionWindow);
        }

        public void openSubOption(int index)
        {
            if(0 <= index && index < subOptionWindows.Count)
            {
                menuCtr.openWindow(subOptionWindows[index]);
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