using Assets.Script.UI;
using Assets.Script.UI.Menu;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script.System.Menu.App
{
    public class App : MonoBehaviour
    {
        [SerializeField]
        private GameObject _window;
        public GameObject Window { get { return _window; } }

        [Space]
        [Header("참조 스크립트")]
        [SerializeField]
        private AppUI _ui;
        public AppUI UI { get { return _ui; } }

        public virtual void open()
        {
            _ui.openApp(_window);
        }

        public virtual bool close()
        {
            _ui.closeApp(_window);

            // 해당 앱이 닫힘
            return true;
        }
    }
}