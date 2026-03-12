using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [SerializeField]
    private Dialogue dialogue;

    public bool IsPrinting => dialogue.IsPrinting;

    /************************************************************
    * [대화 출력]
    * 
    * 인게임 화면의 대화 제어
    ************************************************************/

    public void PrintText(TextLine line)
    {
        var manager = ScenarioManager.Instance;
        var name = manager.GetLocalizedName(line.nameKey);
        var text = manager.GetLocalizedDialogue(line.dialogueKey);

        dialogue.SetName(name);
        dialogue.PrintText(text);
    }

    public void TextSkip()
    {
        dialogue.TextSkip();
    }

    public void CloseDialogue()
    {
        dialogue.SetDialogView(false);
    }
}