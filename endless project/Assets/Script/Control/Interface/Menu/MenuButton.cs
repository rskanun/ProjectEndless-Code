using Assets.Script.UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Script.System.Menu
{
    public class MenuButton : MonoBehaviour, IPointerEnterHandler
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

        public void onButton()
        {
            Debug.Log("click");
            menuManager.iconSelect(thisIcon);
        }

        public void OnPointerEnter(PointerEventData eventdata)
        {
            menuManager.moveSelectTo((int)thisIcon);
            menuManager.setSelectPos(thisIcon);
        }
    }
}