using System.Collections;
using UnityEditor.Search;
using UnityEngine;

namespace Assets.Script.System.Menu.Popup
{
    public abstract class Popup
    {
        public Popup()
        {
            PopupManager.Instance.popupAdd(this);
        }

        public abstract void show();
        public abstract void destroy();
        public abstract void close();
    }
}