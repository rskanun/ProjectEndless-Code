using System.Collections;
using TMPro;
using UnityEngine;

namespace Assets.Script.UI
{
    public class OptionUI : MonoBehaviour
    {
        [SerializeField] private GameObject timePanel;
        [SerializeField] private TextMeshProUGUI timeTxt;
        
        public void setActiveTimePanel(bool isActive)
        {
            timePanel.SetActive(isActive);
        }    
    }
}