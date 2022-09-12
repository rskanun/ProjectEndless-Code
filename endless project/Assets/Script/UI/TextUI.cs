using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TextUI : MonoBehaviour
{
    // Game Object
    public Text textLine;
    public GameObject textDialogue;

    void Awake ()
    {
        // 텍스트와 텍스트창 숨김
        textLine.gameObject.SetActive(false);
        textDialogue.gameObject.SetActive(false);
    }

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
