using System.Collections;
using UnityEngine;

namespace Assets.Script.UI.Menu.App
{
    public class SubWindowUI : MonoBehaviour
    {
        [Header("서브창 취소 패널")]
        [SerializeField] private GameObject cancelPanel;

        public void setCancelPanel(bool isVeiw)
        {
            cancelPanel.SetActive(isVeiw);
        }
    }
}