using Assets.Script.Control;
using Assets.Script.UI;
using System.Collections;
using UnityEngine;

namespace Assets.Script.System.Menu
{
    public class MenuManager : MonoBehaviour
    {
        public MenuUI ui;

        private MenuControl menuCtr;

        // 참조 스크립트가 존재하는 타 오브젝트
        [SerializeField] private GameObject selectIcon;

        private void Awake()
        {
            // init component
            menuCtr = GetComponent<MenuControl>();

            setPhoneUI();
        }

        private void setPhoneUI()
        {
            ui.setService(true);
            ui.setWiFi(true);

            ui.setTime(3, 29);
        }

        protected internal void setSelectPos(menuIcon icon)
        {
            int iconNum = (int)icon;

            menuCtr.setSelectPos(iconNum);
            moveSelectTo(icon);
        }

        protected internal void menuView(bool isView)
        {
            ui.setMenuView(isView);
        }

        protected internal void moveSelectTo(menuIcon icon)
        {
            switch(icon)
            {
                case menuIcon.option:   ui.moveToOption(); break;
                case menuIcon.save:     ui.moveToSave(); break;
                case menuIcon.load:     ui.moveToLoad(); break;
                case menuIcon.title:    ui.moveToTitle(); break;
                case menuIcon.call:     ui.moveToCall(); break;
                case menuIcon.message:  ui.moveToMsg(); break;
                default: break;
            }
        }

        /************************************************************
        * [아이콘]
        * 
        * 메뉴의 각 아이콘의 기능 수행
        ************************************************************/

        protected internal void iconSelect(menuIcon icon)
        {
            switch(icon)
            {
                case menuIcon.option:   option(); break;
                case menuIcon.save:     save(); break;
                case menuIcon.load:     load(); break;
                case menuIcon.title:    title(); break;
                case menuIcon.call:     call(); break;
                case menuIcon.message:  message(); break;
                default: break;
            }
        }

        private void option()
        {
            Debug.Log("option");
        }

        private void save()
        {
            Debug.Log("save");
        }

        private void load()
        {
            Debug.Log("load");
        }

        private void title()
        {
            Debug.Log("title");
        }

        private void call()
        {
            Debug.Log("call");
        }

        private void message()
        {
            Debug.Log("message");
        }
    }
}