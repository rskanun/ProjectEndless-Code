using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TextUI : MonoBehaviour
{
    // Game Object
    public Text textLine;
    public GameObject textDialogue;

    public void setDialogView(bool isView)
    {
        textLine.gameObject.SetActive(isView);
        textDialogue.gameObject.SetActive(isView);
    }

    public void setText(string text)
    {
        textLine.text = text;
    }
}
