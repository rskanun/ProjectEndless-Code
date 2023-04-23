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

        public void addPopup(Popup popup)
        {
            popupList.Add(popup);
        }

        public void destroyPopup(Popup popup)
        {
            int index = popupList.IndexOf(popup);
            if (index != -1)
            {
                popupList.RemoveAt(index);
                popup.destroy();
            }
            else Debug.Log(popup);
        }

        public void destroyPopup()
        {
            if (popupList.Count > 0)
            {
                int index = popupList.Count - 1;
                popupList[index].destroy();

                popupList.RemoveAt(index);
            }
        }
    }
}