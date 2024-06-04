using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToggleUI : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public Toggle toggle;

    public string optionName;

    private void Start()
    {
        // init option name
        onClick();
    }

    public void onClick()
    {
        if (toggle.isOn) nameText.text = optionName + "[ON]";
        else nameText.text = optionName + "[OFF]";
    }
}