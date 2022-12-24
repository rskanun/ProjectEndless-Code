using Assets.Script.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

namespace Assets.Script.System.Menu
{
    public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
    {
        // 참조 스크립트가 있는 오브젝트
        [SerializeField]
        private GameObject menu;

        // 참조 오브젝트
        private MenuManager menuManager;

        // 이 아이콘의 종류
        [SerializeField]
        private menuIcon thisIcon;

        private void Awake()
        {
            // init component
            menuManager = menu.GetComponent<MenuManager>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if(eventData.IsPointerMoving())
            {
                menuManager.setSelectPos(thisIcon);
            }

            else if(Input.GetMouseButtonDown(0))
            {
                menuManager.iconSelect(thisIcon);
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            menuManager.iconSelect(thisIcon);
        }
    }
}