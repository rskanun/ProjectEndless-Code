using Assets.Script.UI;
using System.Collections;
using UnityEngine;

namespace Assets.Script.System.Menu
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField]
        private MenuUI ui;

        protected internal void menuViewOn()
        {
            // UI on
            ui.setMenuView(true);

            // 오브젝트 시간 멈추기
            Time.timeScale = 0;
        }

        protected internal void menuViewOff()
        {
            // UI off
            ui.setMenuView(false);

            // 오브젝트 시간 움직이기
            Time.timeScale = 1;
        }

        public void moveSelectTo(int num)
        {
            menuIcon icon = (menuIcon)num;
            switch(icon)
            {
                case menuIcon.option: ui.moveToOption(); break;
                case menuIcon.save: ui.moveToSave(); break;
                case menuIcon.load: ui.moveToLoad(); break;
                case menuIcon.title: ui.moveToTitle(); break;
                case menuIcon.call: ui.moveToCall(); break;
                case menuIcon.message: ui.moveToMsg(); break;
                default: break;
            }
        }
    }
}