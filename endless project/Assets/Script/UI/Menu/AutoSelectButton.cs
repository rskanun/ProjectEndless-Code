using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.UI.Menu
{
    public class AutoSelectButton : MonoBehaviour
    {
        public Button button;

        private void OnEnable()
        {
            button.Select();
        }
    }
}