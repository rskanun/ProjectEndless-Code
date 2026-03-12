public class SaveApp : NoteApp, INoteState
{
    protected override void OnOpen()
    {
        NoteContext.Instance.SetState(this);

        base.OnOpen();
    }

    public void InitObj()
    {
        UI.UpdateAddSaveButton();
    }

    public void OnClickHandler(int id)
    {
        Manager.SaveHandler(id);
    }
}