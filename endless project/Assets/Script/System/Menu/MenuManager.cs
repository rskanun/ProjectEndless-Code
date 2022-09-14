using Assets.Script.UI;
using System.Collections;
using UnityEngine;

namespace Assets.Script.System.Menu
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField]
        private OptionMenuUI menu;

        bool isView = false;

        public void menuView()
        {
            if (isView)
            {
                menu.setMenuView(false);
                isView = false;
            }

            else
            {
                menu.setMenuView(true);
                isView = true;
            }
        }
    }
}