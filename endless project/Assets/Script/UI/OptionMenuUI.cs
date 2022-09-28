using System.Collections;
using UnityEngine;

namespace Assets.Script.UI
{
    public enum Menu
    {
        load, save, title
    }

    public class OptionMenuUI : MonoBehaviour
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

        public void moveIcon(Menu selectIcon)
        {
            switch(selectIcon)
            {
                case UI.Menu.load:
                    setCursor(loadIcon);
                    break;

                case UI.Menu.save:
                    setCursor(saveIcon);
                    break;

                case UI.Menu.title:
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