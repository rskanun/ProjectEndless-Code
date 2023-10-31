using Assets.Script.UI.Menu.App;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
using UnityEngine;

namespace Assets.Script.System.Menu.App
{
    public class SubWindowApp : App
    {
        [SerializeField]
        private SubWindowUI _subWindowUI;
        protected Stack<GameObject> subWindows = new Stack<GameObject>();

        public virtual void subOpen(GameObject subWindow)
        {
            subWindow.SetActive(true);
            subWindows.Push(subWindow);

            _subWindowUI.setCancelPanel(true);
        }

        public override bool close()
        {
            if (subWindows.Count > 0)
            {
                GameObject subWindow = subWindows.Pop();
                subWindow.SetActive(false);

                if (subWindows.Count <= 0)
                    _subWindowUI.setCancelPanel(false);

                // 하위 서브 윈도우가 전부 안 닫힘
                return false;
            }
            else
            {
                return base.close();
            }
        }
    }
}