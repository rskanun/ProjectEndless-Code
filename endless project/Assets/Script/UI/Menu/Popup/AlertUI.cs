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

        public void setActive(bool isActive)
        {
            darkPanel.SetActive(isActive);

            if (isActive) AppAnimation.popupOpenAnimation(popup);
            else AppAnimation.popupCloseAnimation(popup)
                    .OnComplete(() => Destroy(gameObject));
        }

        public void setAlert(string msg, string okText)
        {
            contents.text = msg;
            okTxt.text = okText;
        }

        public void onClick()
        {
            setActive(false);
        }
    }
}