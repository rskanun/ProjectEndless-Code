using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [Header("대화창 종류")]
    [SerializeField] private Dialogue normalDialogue;
    [SerializeField] private Dialogue abyssDialogue;

    public bool IsPrinting => current.IsPrinting;

    private Dialogue current;

    public void OnMapChanged()
    {
        MapData map = GameData.Instance.CurrentMap;

        // 현재 플레이어가 있는 위치에 따라 대화창 스킨 바꾸기
        current = map.IsAbyss ? abyssDialogue : normalDialogue;
    }

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

        current.SetName(name);
        current.PrintText(text);
    }

    public void TextSkip()
    {
        current.TextSkip();
    }

    public void CloseDialogue()
    {
        current.SetDialogView(false);
    }
}