using Assets.Script.System;
using Assets.Script.System.Menu;
using Assets.Script.UI;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEditor.PackageManager.UI;
using UnityEngine;

namespace Assets.Script.Control
{
    public class MenuControl : InterfaceControl
    {
        public GameObject menu;

        public List<GameObject> windows = new List<GameObject>();

        // 참조 스크립트
        private MenuManager menuManager;

        /************************************************************
        * [Init]
        * 
        * 각종 초기 변수 및 함수 선언
        ************************************************************/

        // 선택 가능한 아이콘의 가로 세로 갯수
        private const int X = 3;
        private const int Y = 2;

        private void Start()
        {
            // init value
            valueReset();

            // init component
            menuManager = menu.GetComponent<MenuManager>();
            setIconPoint(X, Y);
        }

        /************************************************************
        * [메뉴]
        * 
        * 메뉴 상태에서 커서 이동을 제어
        ************************************************************/

        private void Update()
        {
            menuKeyPress();
            interfaceKeyPress();
        }

        private void menuKeyPress()
        {
            // 메뉴 활성화/비활성화
            if (Input.GetKeyDown(Option.menu))
            {
                if (menu.activeSelf == false) // 메뉴가 켜져있지 않은 경우
                {
                    menu.SetActive(true);
                    windows.Add(menu);

                }
                else if (windows.Count <= 1) // 메뉴만 켜져있는 경우
                {
                    menu.SetActive(false);
                }
            }
        }

        protected internal override void iconSelect(int x, int y)
        {
            int index = (y * X) + x;
            menuManager.iconSelect((menuIcon)index);
        }

        protected internal override void moveUI(int x, int y)
        {
            int index = (y * X) + x;
            menuManager.moveSelectTo((menuIcon)index);
        }

        protected internal void setSelectPos(int num)
        {
            int x = num % X;
            int y = num / X;

            selectPoint.x = x;
            selectPoint.y = y;
        }
    }
}