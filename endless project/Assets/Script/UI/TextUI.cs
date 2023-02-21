using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextUI : MonoBehaviour
{
    private bool isActive = false;
    public bool IsActive { get { return isActive; } }

    // Game Object
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI textLine;
    public GameObject textDialogue;
    public GameObject touchPanel;

    public void setDialogView(bool isView)
    {
        isActive = isView;

        nameText.gameObject.SetActive(isView);
        textLine.gameObject.SetActive(isView);
        textDialogue.gameObject.SetActive(isView);
        touchPanel.SetActive(isView);
    }

    public void setText(string text)
    {
        textLine.text = text;
    }

    public void setName(string name)
    {
        nameText.text = name;
    }

    public void textClear()
    {
        textLine.text = "";
    }
}
