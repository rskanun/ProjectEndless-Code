using UnityEngine;

public class LoadApp : NoteApp, INoteState
{
    protected override void OnOpened()
    {
        NoteContext.Instance.SetState(this);

        base.OnOpened();
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