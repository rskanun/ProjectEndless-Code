using System.Collections;
using UnityEngine;

namespace Assets.Script.UI
{
    public enum menuIcon
    {
        load, save, title
    }

    public class MenuUI : MonoBehaviour
    {
        public GameObject menu;
        public GameObject cursor;

        public GameObject loadIcon;
        public GameObject saveIcon;
        public GameObject titleIcon;

        void Awake()
        {
            // 옵션 메뉴 숨기기
            menu.gameObject.SetActive(false);
        }

        public void setMenuView(bool isView)
        {
            menu.gameObject.SetActive(isView);
        }

        public void moveIcon(menuIcon selectIcon)
        {
            switch(selectIcon)
            {
                case UI.menuIcon.load:
                    setCursor(loadIcon);
                    break;

                case UI.menuIcon.save:
                    setCursor(saveIcon);
                    break;

                case UI.menuIcon.title:
                    setCursor(titleIcon);
                    break;
            }
        }

        private void setCursor(GameObject select)
        {
            cursor.transform.localPosition = select.transform.localPosition;
        }
    }
}