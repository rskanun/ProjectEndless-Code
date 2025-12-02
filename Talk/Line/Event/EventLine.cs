using UnityEngine;

public class EventLine : Line
{
    [SerializeReference]
    private IDialogueEvent _dialogueEvent;
    public IDialogueEvent dialogueEvent => _dialogueEvent;

    public EventLine(IDialogueEvent dialogueEvent) : base(LineType.Event)
    {
        _dialogueEvent = dialogueEvent;
    }

#if UNITY_EDITOR
    public EventLine(string guid, IDialogueEvent dialogueEvent) : base(guid, LineType.Event)
    {
        _dialogueEvent = dialogueEvent;
    }
#endif
}