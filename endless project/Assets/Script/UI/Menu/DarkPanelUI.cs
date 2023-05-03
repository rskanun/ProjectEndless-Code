using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.UI.Menu
{
    public class DarkPanelUI : MonoBehaviour
    {
        [SerializeField]
        private Image darkPanel;

        private void Start ()
        {
            darkPanel.color = DarkPanelSetting.Instance.PanelColor;
        }
    }
}