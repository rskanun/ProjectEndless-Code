using System.Collections;
using TMPro;
using UnityEngine;

namespace Assets.Script.UI.Menu.Popup
{
    public class ConfirmUI : MonoBehaviour
    {
        [SerializeField] private GameObject popup;
        [SerializeField] private GameObject darkPanel;
        [SerializeField] private TextMeshProUGUI contents;
        [SerializeField] private TextMeshProUGUI yesTxt;
        [SerializeField] private TextMeshProUGUI noTxt;

        public delegate void popupCallBack();
        private event popupCallBack yesCallBack;
        private event popupCallBack noCallBack;

        public void setActive(bool isActive)
        {
            darkPanel.SetActive(isActive);

            if (isActive) AppAnimation.popupOpenAnimation(popup);
            else AppAnimation.popupCloseAnimation(popup);
        }

        public void setContents(string msg)
        {
            contents.text = msg;
        }

        public void setYesText(string yesText)
        {
            yesTxt.text = yesText;
        }

        public void setNoText(string noText)
        {
            noTxt.text = noText;
        }

        public void setConfirm(string msg, string yesText = "네", string noText = "아니요")
        {
            contents.text = msg;
            yesTxt.text = yesText;
            noTxt.text = noText;
        }

        public void setYesCallBack(popupCallBack listener)
        {
            yesCallBack += listener;
        }

        public void setNoCallBack(popupCallBack listener)
        {
            noCallBack += listener;
        }

        public void onYes()
        {
            yesCallBack?.Invoke();
        }

        public void onNo()
        {
            noCallBack?.Invoke();
        }
    }
}