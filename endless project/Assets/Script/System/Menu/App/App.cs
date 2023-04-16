using Assets.Script.UI;
using Assets.Script.UI.Menu;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script.System.Interface.Menu.App
{
    public class App : MonoBehaviour
    {
        [SerializeField]
        private GameObject _window;
        public GameObject Window { get { return _window; } }
        protected Stack<GameObject> subWindows = new Stack<GameObject>();

        [Space]
        [Header("참조 스크립트")]
        [SerializeField]
        private AppUI _ui;
        public AppUI UI { get { return _ui; } }

        public virtual void open()
        {
            _ui.openApp(_window);
        }

        public virtual void subOpen(GameObject subWindow)
        {
            subWindow.SetActive(true);
            subWindows.Push(subWindow);

            _ui.setCancelPanel(true);
        }

        public virtual bool close()
        {
            if (subWindows.Count > 0)
            {
                GameObject subWindow = subWindows.Pop();
                subWindow.SetActive(false);

                if (subWindows.Count <= 0)
                    _ui.setCancelPanel(false);

                // 하위 서브 윈도우가 전부 안 닫힘
                return false;
            }
            else
            {
                _ui.closeApp(_window);

                // 해당 앱이 닫힘
                return true;
            }
        }
    }
}