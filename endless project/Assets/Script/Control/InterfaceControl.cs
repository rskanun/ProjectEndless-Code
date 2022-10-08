using Assets.Script.System.Menu;
using System.Collections;
using UnityEngine;

namespace Assets.Script.Control
{
    public class InterfaceControl : MonoBehaviour
    {
        // 참조 스크립트
        private MenuManager menuManager;

        // 참조 스크립트가 존재하는 타 오브젝트
        [SerializeField] private GameObject menuUI;

        // 현재 선택한 것에 대한 좌표
        private Vector2 select_point = Vector2.zero;

        // 현재 인터페이스 내에서 컨트롤을 하는 중인가
        private bool isInterface = false;


        /************************************************************
        * [Key Value]
        * 
        * 각종 키들의 string을 모아둔 변수
        ************************************************************/

        // 선택키
        private string select = Option.getKey(Key.select);

        // 취소키
        private string cancel = Option.getKey(Key.cancel);

        // 메뉴(ESC)키
        private string menu = Option.getKey(Key.menu);

        /************************************************************
        * [Init]
        * 
        * 각종 초기 변수 및 함수 선언
        ************************************************************/

        private void Awake()
        {
            initComponent(); 
        }

        private void initComponent()
        {
            // UI Canvas -> Menu
            menuManager = menuUI.GetComponent<MenuManager>();
        }

        private void Update()
        {
            cursorMoveKeyPress();
            menuKeyPress();
        }

        /************************************************************
        * [메뉴]
        * 
        * 메뉴 상태에서 커서 이동을 제어
        ************************************************************/

        private void menuKeyPress()
        {
            // 메뉴키
            if(Input.GetKeyDown(menu))
            {
                // bool switching
                isInterface = !isInterface;
                menuManager.menuViewSwitch(isInterface);
            }
        }

        private void cursorMoveKeyPress()
        {

        }
    }
}