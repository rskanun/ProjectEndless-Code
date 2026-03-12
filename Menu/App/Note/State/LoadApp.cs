using UnityEngine;

public class LoadApp : NoteApp, INoteState
{
    protected override void OnOpen()
    {
        NoteContext.Instance.SetState(this);

        base.OnOpen();
    }

    public void InitObj()
    {
        UI.InitNotice();
    }

    public void OnClickHandler(int id)
    {
        Manager.LoadHandler(id);
    }
}