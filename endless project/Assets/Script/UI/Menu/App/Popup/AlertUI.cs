using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Assets.Script.UI.Menu.Popup
{
    public class AlertUI : MonoBehaviour
    {
        [SerializeField] private GameObject popup;
        [SerializeField] private GameObject darkPanel;
        [SerializeField] private TextMeshProUGUI contents;
        [SerializeField] private TextMeshProUGUI okTxt;

        public delegate void PopupCallBack();
        private event PopupCallBack okCallBack;

        public void setAlert(string msg, string okText)
        {
            contents.text = msg;
            okTxt.text = okText;
        }

        public void setOkCallBack(PopupCallBack listener)
        {
            okCallBack = listener;
        }

        public void onClick()
        {
            okCallBack?.Invoke();
        }

        public Sequence show()
        {
            darkPanel.SetActive(true);

            return AppAnimation.popupOpenAnimation(popup);
        }

        public Sequence hide()
        {
            darkPanel.SetActive(false);

            return AppAnimation.popupCloseAnimation(popup);
        }
    }
}