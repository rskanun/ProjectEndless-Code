using Assets.Script.Interface.Menu.App;
using Assets.Script.System.Menu;
using Assets.Script.UI;
using UnityEngine;

namespace Assets.Script.Control
{
    public class MenuControl : InterfaceControl
    {
        [Space]
        [Header("참조 스크립트")]
        public MenuManager menuManager;
        public HomeScreen homeScreen;

        /************************************************************
        * [Init]
        * 
        * 각종 초기 변수 및 함수 선언
        ************************************************************/

        // 선택 가능한 아이콘의 가로 세로 갯수
        private const int X = 3;
        private const int Y = 2;

        // 메뉴 창의 켜짐 여부
        private bool isOpen = false;

        private void Start()
        {
            option = OptionSetting.Instance;

            // init value
            valueReset();

            // init component
            setIconPoint(X, Y);
        }

        /************************************************************
        * [메뉴 제어]
        * 
        * 키보드를 이용한 메뉴 상태에서의 제어
        ************************************************************/

        private void Update()
        {
            menuKeyPress();
            interfaceKeyPress();
        }

        private void menuKeyPress()
        {
            // 메뉴 활성화/비활성화
            if (Input.GetKeyDown(option.Menu) && homeScreen.isAppEmpty && homeScreen.playAnimation == false)
            {
                if(isOpen == false) homeScreen.open();
                else homeScreen.close();

                isOpen = !isOpen; // switching
            }
        }

        public override void cancel()
        {
            // 메인 화면에 앱이 켜져있는 경우 캔슬키로 작동
            if(homeScreen.isAppEmpty == false)
            {
                homeScreen.cancel();
            }
            // 메뉴키와 캔슬키가 다를 경우
            // 메인 화면에서 캔슬키 작동시 메뉴 닫힘
            else if(option.Cancel != option.Menu && homeScreen.playAnimation == false)
            {
                homeScreen.close();
            }
        }

        protected override void iconSelect(int x, int y)
        {
            int index = (y * X) + x;
            menuManager.iconSelect((MenuIcon)index);
        }
    }
}