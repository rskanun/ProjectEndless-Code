public class SaveApp : NoteApp, INoteState
{
    protected override void OnOpened()
    {
        NoteContext.Instance.SetState(this);

        base.OnOpened();
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