using System.Collections;
using UnityEngine;

namespace Assets.Script.UI
{
    public enum menuIcon
    {
        option, save, load,
        title, call, message
    }

    public class MenuUI : MonoBehaviour
    {
        public GameObject menu;
        public GameObject cursor;

        public GameObject optionIcon;
        public GameObject saveIcon;
        public GameObject loadIcon;
        public GameObject titleIcon;
        public GameObject callIcon;
        public GameObject msgIcon;

        void Awake()
        {
            // 옵션 메뉴 숨기기
            menu.gameObject.SetActive(false);
        }

        protected internal void setMenuView(bool isView)
        {
            menu.gameObject.SetActive(isView);
        }

        /************************************************************
        * [커서 이동]
        * 
        * 아이콘을 가리키는 커서 이동
        ************************************************************/

        protected internal void moveToOption() { setCursor(optionIcon); }
        protected internal void moveToSave() { setCursor(saveIcon); }
        protected internal void moveToLoad() { setCursor(loadIcon); }
        protected internal void moveToTitle() { setCursor(titleIcon); }
        protected internal void moveToCall() { setCursor(callIcon); }
        protected internal void moveToMsg() { setCursor(msgIcon); }

        private void setCursor(GameObject select)
        {
            cursor.transform.localPosition = select.transform.localPosition;
        }
    }
}