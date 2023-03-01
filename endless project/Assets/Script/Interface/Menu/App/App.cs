using Assets.Script.Control;
using Assets.Script.UI;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script.Interface.Menu.App
{
    public class App : MonoBehaviour
    {
        public GameObject window;
        protected Stack<GameObject> subWindows = new Stack<GameObject>();
        [Space]
        [Header("참조 스크립트")]
        public MenuControl menuCtr;
        public AppAnimation appAnimation;

        public virtual void open()
        {
            appAnimation.openAppAnimation(window);
        }

        public virtual void subOpen(GameObject subWindow)
        {
            subWindow.SetActive(true);
            subWindows.Push(subWindow);
        }

        public virtual bool close()
        {
            if (subWindows.Count > 0)
            {
                GameObject subWindow = subWindows.Pop();
                subWindow.SetActive(false);

                // 하위 서브 윈도우가 전부 안 닫힘
                return false;
            }
            else
            {
                appAnimation.closeAppAnimation(window);

                // 해당 앱이 닫힘
                return true;
            }
        }
    }
}