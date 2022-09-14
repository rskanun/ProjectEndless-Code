using System.Collections;
using UnityEngine;

namespace Assets.Script.UI
{
    public class OptionMenuUI : MonoBehaviour
    {
        public GameObject menu;

        void Awake()
        {
            // 옵션 메뉴 숨기기
            menu.gameObject.SetActive(false);
        }

        public void setMenuView(bool isView)
        {
            menu.gameObject.SetActive(isView);
        }
    }
}