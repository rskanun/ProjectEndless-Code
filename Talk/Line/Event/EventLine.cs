using UnityEngine;

public class EventLine : Line
{
    [SerializeField]
    private IDialogueEvent _dialogueEvent;
    public IDialogueEvent dialogueEvent => _dialogueEvent;

    public EventLine(IDialogueEvent dialogueEvent) : base(LineType.Event)
    {
        _dialogueEvent = dialogueEvent;
    }

#if UNITY_EDITOR
    public EventLine(EventNodeData nodeData) : base(nodeData.guid, LineType.Event)
    {

    }
#endif
}