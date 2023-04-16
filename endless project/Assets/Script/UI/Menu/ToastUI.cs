using System.Collections;
using TMPro;
using UnityEngine;

namespace Assets.Script.UI.Menu
{
    public class ToastUI : MonoBehaviour
    {
        [SerializeField] private GameObject toastMsg;
        [SerializeField] private TextMeshProUGUI contents;
        
        public void makeMsg(string msg)
        {
            if(toastMsg.activeSelf == true)
                setActive(false);

            contents.text = msg;
            AppAnimation.toastAnimation(toastMsg, 0.15f, 1.5f);
        }

        public void setActive(bool isActive)
        {
            toastMsg.SetActive(isActive);
        }
    }
}