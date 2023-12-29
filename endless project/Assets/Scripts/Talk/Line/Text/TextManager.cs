using UnityEngine;

public class TextManager : MonoBehaviour
{
    [Header("참조 스크립트")]
    [SerializeField] private TextUI ui;

    public bool IsPrinting
        => ui.IsPrinting;

    /************************************************************
    * [대화 출력]
    * 
    * 인게임 화면의 대화 제어
    ************************************************************/

    public void PrintText(TextLine line)
    {
        ui.SetName(line.name);
        ui.PrintText(line.text);
    }

    public void TextSkip()
    {
        ui.TextSkip();
    }

    public void TextDestroy()
    {
        ui.SetDialogView(false);
    }
}