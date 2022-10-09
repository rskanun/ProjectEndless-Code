using Assets.Script.System.Menu;
using System.Collections;
using UnityEngine;

namespace Assets.Script.Control
{
    public class MenuControl : InterfaceControl
    {
        // 참조 스크립트
        private MenuManager menuManager;

        // 참조 스크립트가 존재하는 타 오브젝트
        [SerializeField] private GameObject menuUI;

        /************************************************************
        * [Key Value]
        * 
        * 각종 키들의 string을 모아둔 변수
        ************************************************************/

        // 메뉴(ESC)키
        private string menu = Option.getKey(Key.menu);

        /************************************************************
        * [Init]
        * 
        * 각종 초기 변수 및 함수 선언
        ************************************************************/

        // 선택 가능한 아이콘의 가로 세로 갯수
        private const int X = 3;
        private const int Y = 2;

        private void Awake()
        {
            InitComponent();
            setIconPoint(X - 1, Y - 1);
        }

        private void InitComponent()
        {
            // UI Canvas -> Menu
            menuManager = menuUI.GetComponent<MenuManager>();
        }

        private void Update()
        {
            interfaceKeyPress();
            menuKeyPress();
        }

        /************************************************************
        * [메뉴]
        * 
        * 메뉴 상태에서 커서 이동을 제어
        ************************************************************/

        private int openNum = 0;

        private void menuKeyPress()
        {
            // 메뉴키
            if(Input.GetKeyDown(menu))
            {
                if (!isInterface) menuOn();
                else menuOff();
            }
        }

        private void menuOn()
        {
            isInterface = true;

            menuManager.menuViewOn();
        }

        private void menuOff()
        {
            isInterface = false;

            valueReset(); // 변수 리셋
            menuManager.menuViewOff();
        }

        protected internal override void iconSelect(int x, int y)
        {
            throw new global::System.NotImplementedException();
        }

        protected internal override void iconCancel()
        {
            // 메뉴만 켜져있는 경우
            if (openNum == 1)
            {
                openNum--;
                menuOff();
            }
        }

        protected internal override void moveUI(int x, int y)
        {
            int index = (y * X) + x;
            menuManager.moveSelectTo(index);
        }
    }
}