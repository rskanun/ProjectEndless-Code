using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.UI
{
    public class SliderUI : MonoBehaviour
    {
        public TextMeshProUGUI nameText;
        public Slider slider;

        public float value;
        public string optionName;

        private void Start()
        {
            // init checkbox
            slider.value = value;

            // init option name
            nameText.text = optionName + " : " + value*100;
        }

        public void percentUpdate()
        {
            nameText.text = optionName + " : " + (int)(slider.value * 100) + "%";
        }
    }
}