using Assets.Script.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

namespace Assets.Script.System.Menu
{
    public class ButtonBlink : MonoBehaviour, IPointerMoveHandler
    {
        // 참조 오브젝트
        public MenuManager menuManager;

        // 이 아이콘의 종류
        public MenuIcon thisIcon;

        public void OnPointerMove(PointerEventData eventData)
        {
            if(eventData.IsPointerMoving())
            {
                menuManager.setSelectPos(thisIcon);
            }
        }
    }
}