using System.Collections;
using TMPro;
using UnityEngine;

namespace Assets.Script.UI.Menu
{
    public class ToastUI : MonoBehaviour
    {
        [SerializeField] private GameObject toastMsg;
        [SerializeField] private TextMeshProUGUI contents;

        private float openDelay = 0.15f;
        private float closeDelay = 0.15f;
        
        public void makeMsg(string msg)
        {
            if(toastMsg.activeSelf == true)
                setActive(false);

            contents.text = msg;
            AppAnimation.toastAnimation(toastMsg, openDelay, closeDelay);
        }

        public void setActive(bool isActive)
        {
            toastMsg.SetActive(isActive);
        }
    }
}