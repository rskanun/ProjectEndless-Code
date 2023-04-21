using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.UI.Menu.Popup
{
    public class ConfirmUI : MonoBehaviour
    {
        [SerializeField] private GameObject popup;
        [SerializeField] private GameObject darkPanel;
        [SerializeField] private TextMeshProUGUI contents;
        [Space]
        [Header("확인 버튼")]
        [SerializeField] private Button yesBtn;
        [SerializeField] private TextMeshProUGUI yesTxt;
        [Space]
        [Header("취소 버튼")]
        [SerializeField] private Button noBtn;
        [SerializeField] private TextMeshProUGUI noTxt;

        public delegate void PopupCallBack();
        private event PopupCallBack yesCallBack;
        private event PopupCallBack noCallBack;

        public void setActive(bool isActive)
        {
            darkPanel.SetActive(isActive);

            if (isActive) AppAnimation.popupOpenAnimation(popup);
            else AppAnimation.popupCloseAnimation(popup)
                    .OnComplete(() => Destroy(gameObject));
        }

        public void setConfirm(string msg, string yesText, string noText)
        {
            contents.text = msg;
            yesTxt.text = yesText;
            noTxt.text = noText;
        }

        public void setYesCallBack(PopupCallBack listener)
        {
            yesCallBack += listener;
        }

        public void setNoCallBack(PopupCallBack listener)
        {
            noCallBack += listener;
        }

        public void onYes()
        {
            yesCallBack?.Invoke();
            setActive(false);
        }

        public void onNo()
        {
            noCallBack?.Invoke();
            setActive(false);
        }
    }
}