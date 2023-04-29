using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script.System.Menu.Popup
{
    public class PopupManager : MonoBehaviour
    {
        private List<Popup> popupList = new List<Popup>();
        public bool noMorePopup { get { return !(popupList.Count > 0); } }

        private static PopupManager _instance;
        public static PopupManager Instance { get { return _instance; } }

        private void Awake()
        {
            _instance = this;
        }

        public void popupAdd(Popup popup)
        {
            popupList.Add(popup);
        }

        public void popupDestroy(Popup popup)
        {
            popupList.Remove(popup);
            popup.destroy();
        }

        public void popupClose()
        {
            if (popupList.Count > 0)
            {
                int index = popupList.Count - 1;
                popupList[index].close();
            }
        }
    }
}