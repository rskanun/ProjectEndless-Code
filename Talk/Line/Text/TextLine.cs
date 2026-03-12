using UnityEngine;

[System.Serializable]
public class TextLine : Line
{
    [SerializeField]
    private string _name;
    public string name => _name;

    [SerializeField]
    private string _nameKey;
    public string nameKey => _nameKey;

    [SerializeField]
    private string _dialogue;
    public string dialogue => _dialogue;

    [SerializeField]
    private string _dialogueKey;
    public string dialogueKey => _dialogueKey;

    public TextLine(string name, string text) : base(LineType.Text)
    {
        _name = name;
        _dialogue = text;
    }

#if UNITY_EDITOR
    public TextLine(TextNodeData nodeData) : base(nodeData.guid, LineType.Text)
    {
        _name = nodeData.speaker;
        _nameKey = nodeData.speakerKey;

        _dialogue = nodeData.dialogue;
        _dialogueKey = nodeData.dialogueKey;
    }
#endif
}