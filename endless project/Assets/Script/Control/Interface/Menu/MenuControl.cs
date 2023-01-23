using Assets.Script.System.Menu;
using Assets.Script.UI;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.Control
{
    public class MenuControl : InterfaceControl
    {
        public Stack<GameObject> windows = new Stack<GameObject>();

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
            menuManager = interfaceWindow.GetComponent<MenuManager>();
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
                // 메뉴가 닫혀있는 경우 열기
                if(interfaceWindow.activeSelf == false)
                {
                    openWindow(interfaceWindow);
                }
                else if(windows.Count <= 1)
                {
                    closeWindow();
                }
                // 메뉴키를 눌렀을 경우 열려있는 모든 창 한 번에 닫으며 메뉴 비활성화(패드 전용)
                else if(Option.menu != Option.cancel)
                {
                    allClose();
                    closeWindow();
                }
            }
        }

        public void allClose()
        {
            GameObject window = null;
            while(windows.Count > 1) // 메뉴창은 남겨두기
            {
                window = windows.Pop();
                window.SetActive(false);
            }
        }

        public void openWindow(GameObject window)
        {
            window.SetActive(true);
            windows.Push(window);
        }

        public override void cancel()
        {
            // 켜져있는 창이 하나 이상일 경우에만 작동
            // * 메뉴키와 캔슬키가 같은 경우 메뉴키 관리 부분에서 따로 작동
            if(windows.Count > 1 || Option.cancel != Option.menu && windows.Count == 1)
            {
                closeWindow();
            }
        }

        private void closeWindow()
        {
            GameObject closeWindow = windows.Pop();
            closeWindow.SetActive(false);
        }

        protected internal override void iconSelect(int x, int y)
        {
            int index = (y * X) + x;
            menuManager.iconSelect((MenuIcon)index);
        }

        protected internal override void moveUI(int x, int y)
        {
            int index = (y * X) + x;
            menuManager.moveSelectTo((MenuIcon)index);
        }

        protected internal void setSelectPos(int num)
        {
            int x = num % X;
            int y = num / X;

            setSelectPoint(x, y);
        }
    }
}