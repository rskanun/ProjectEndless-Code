using Assets.Script.System.Menu;
using Assets.Script.System.Menu.Popup;
using UnityEngine;

namespace Assets.Script.System.Menu
{
    public class MenuControl : MonoBehaviour
    {
        // 참조 스크립트
        private MenuManager menuManager;
        
        // 참조 스크립터블 오브젝트
        private OptionSetting option;
        private PlayerState playerState;

        private void Start()
        {
            menuManager = MenuManager.Instance;

            option = OptionSetting.Instance;
            playerState = PlayerState.Instance;
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
                if (playerState.IsMenuActive == false) menuManager.menuOpen();
                else menuManager.menuClose();
            }
        }

        public void cancelKeyPress()
        {
            if (Input.GetKeyDown(option.Cancel) && playerState.AllowCancelKey == true)
            {
                // 메인 화면에 앱이 켜져있는 경우 캔슬키로 작동
                if (menuManager.IsAppEmpty == false || PopupManager.Instance.noMorePopup == false)
                {
                    cancel();
                }
                // 메뉴키와 캔슬키가 다를 경우
                // 메인 화면에서 캔슬키 작동시 메뉴 닫힘
                else if (option.Cancel != option.Menu && menuManager.IsMenuControlable)
                {
                    menuManager.menuClose();
                    playerState.IsMenuActive = false;
                }
            }
        }

        private void cancel()
        {
            // 팝업창 우선 제거
            if (PopupManager.Instance.noMorePopup == false)
            {
                PopupManager.Instance.popupClose();
            }
            else menuManager.appClose();
        }
    }
}