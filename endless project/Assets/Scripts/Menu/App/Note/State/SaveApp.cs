public class SaveApp : NoteApp, INoteState
{
    protected override void LoadData()
    {
        NoteContext.Instance.SetState(this);

        base.LoadData();
    }

    public void InitObj()
    {
        ui.UpdateAddSaveButton();
    }

    public void OnClickHandler(int id)
    {
        manager.SaveHandler(id);
    }
}