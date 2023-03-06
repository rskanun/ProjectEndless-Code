using System.Collections;
using UnityEngine;

namespace Assets.Script.UI
{
    public class AppUI : MonoBehaviour
    {
        public GameObject AppBackground;

        /************************************************************
        * [앱 애니메이션]
        * 
        * 애니메이션 조작 관리
        ************************************************************/

        public void openApp(GameObject window)
        {
            AppAnimation.openAppAnimation(window, AppBackground);
        }

        public void closeApp(GameObject window)
        {
            AppAnimation.closeAppAnimation(window, AppBackground);
        }

        public void openAppSimple(GameObject window)
        {
            AppAnimation.openSimpleAppAnimation(window, AppBackground);
        }

        public void openMenu(GameObject window, float openRotate, float closeRotate)
        {
            AppAnimation.openMenuAnimation(window, openRotate, closeRotate);
        }

        public void closeMenu(GameObject window, float openRotate, float closeRotate)
        {
            AppAnimation.closeMenuAnimation(window, openRotate, closeRotate);
        }

        public void alertOn(GameObject alert)
        {
            AppAnimation.alertOnAnimation(alert);
        }

        public void alertOff(GameObject alert)
        {
            AppAnimation.alertOffAnimation(alert);
        }
    }
}