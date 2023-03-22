using Assets.Script.Control.Text;
using Assets.Script.Interface.Menu.App;
using Assets.Script.System;
using Assets.Script.System.Menu;
using Assets.Script.UI;
using UnityEditor.PackageManager.UI;
using UnityEngine;

namespace Assets.Script.Control
{
    public class MenuControl : MonoBehaviour
    {
        [Header("참조 스크립트")]
        public MenuManager menuManager;

        private OptionSetting option;
        private NoKeyDown noKeyDown;

        private void Start()
        {
            option = OptionSetting.Instance;
            noKeyDown = NoKeyDown.Instance;
        }

        /************************************************************
        * [메뉴 제어]
        * 
        * 키보드를 이용한 메뉴 상태에서의 제어
        ************************************************************/

        private void Update()
        {
            menuKeyPress();
            cancelKeyPress();
        }

        private void menuKeyPress()
        {
            // 메뉴 활성화/비활성화
            if (Input.GetKeyDown(option.Menu) && menuManager.IsMenuControlable)
            {
                if (noKeyDown.IsMenuActive == false) menuManager.menuOpen();
                else menuManager.menuClose();
            }
        }

        public void cancelKeyPress()
        {
            if (Input.GetKeyDown(option.Cancel) && noKeyDown.IsMenuActive == true)
            {
                // 메인 화면에 앱이 켜져있는 경우 캔슬키로 작동
                if (menuManager.IsAppEmpty == false)
                {
                    menuManager.appClose();
                }
                // 메뉴키와 캔슬키가 다를 경우
                // 메인 화면에서 캔슬키 작동시 메뉴 닫힘
                else if (option.Cancel != option.Menu && menuManager.IsMenuControlable)
                {
                    menuManager.menuClose();
                    noKeyDown.IsMenuActive = false;
                }
            }
        }
    }
}