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

        public void setActive(bool isActive)
        {
            darkPanel.SetActive(isActive);

            if (isActive) AppAnimation.popupOpenAnimation(popup);
            else AppAnimation.popupCloseAnimation(popup);
        }

        public void setAlert(string msg, string okText = "확인")
        {
            contents.text = msg;
            okTxt.text = okText;
        }

        public void onClick()
        {
            popup.SetActive(false);
        }
    }
}