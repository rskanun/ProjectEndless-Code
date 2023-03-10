using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.UI.Menu
{
    public class AutoSelectButton : MonoBehaviour
    {
        private void OnEnable()
        {
            Button button = GetComponent<Button>();

            button.Select();
        }
    }
}