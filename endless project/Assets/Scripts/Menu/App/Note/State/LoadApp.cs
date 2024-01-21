using UnityEngine;

public class LoadApp : NoteApp, INoteState
{
    protected override void LoadData()
    {
        NoteContext.Instance.SetState(this);

        base.LoadData();
    }

    public void InitObj()
    {
        ui.InitNotice();
    }

    public void OnClickHandler(int id)
    {
        manager.LoadHandler(id);
    }
}