using Assets.Script.UI;
using System.Collections;
using UnityEngine;

namespace Assets.Script.System.Menu
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField]
        private MenuUI ui;

        public void menuViewSwitch(bool isView)
        {
            if (isView)
            {
                // UI on
                ui.setMenuView(true);

                // 오브젝트 시간 멈추기
                Time.timeScale = 1;
            }

            else
            {
                // UI off
                ui.setMenuView(false);

                // 오브젝트 시간 움직이기
                Time.timeScale = 1;
            }
        }

        public void selectIcon(int num)
        {
            ui.moveIcon((menuIcon)num);
        }

        public void load()
        {
            Debug.Log("load");
        }

        public void save()
        {
            Debug.Log("save");
        }

        public void toTitle()
        {
            Debug.Log("toTitle");
        }
    }
}