using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToggleGroupUI : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI selectText;

    public GameObject optionList;

    public ToggleGroup toggles;

    public string optionName;

    public void onClick()
    {
        Toggle toggle = toggles.ActiveToggles().FirstOrDefault();
        if (toggle.name.Equals("Option1 Toggle")) selectText.text = "Select Option1";
        else if (toggle.name.Equals("Option2 Toggle")) selectText.text = "Select Option2";
        else if (toggle.name.Equals("Option3 Toggle")) selectText.text = "Select Option3";
        else selectText.text = "Select Option Name";
    }
}