using System.Collections;
using UnityEngine;

namespace Assets.Script.UI.Menu
{
    public class HomeScreenUI : MonoBehaviour
    {
        [Header("앱 버튼")]
        [SerializeField] private GameObject optionButton;
        [SerializeField] private GameObject saveButton;
        [SerializeField] private GameObject loadButton;
        [SerializeField] private GameObject titleButton;
        [SerializeField] private GameObject callButton;
        [SerializeField] private GameObject messageButton;

        public void setAllAppButton(bool isActive)
        {
            optionButton.SetActive(isActive);
            saveButton.SetActive(isActive);
            loadButton.SetActive(isActive);
            titleButton.SetActive(isActive);
            callButton.SetActive(isActive);
            messageButton.SetActive(isActive);
        }
    }
}