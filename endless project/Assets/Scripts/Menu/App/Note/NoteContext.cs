public class NoteContext
{
    private static NoteContext _instance;
    public static NoteContext Instance
    {
        get
        {
            if ( _instance == null )
            {
                _instance = new NoteContext();
            }

            return _instance;
        }
    }

    private INoteState state;

    public void SetState(INoteState state)
    {
        this.state = state;
    }

    public void InitAdditionalObj()
    {
        state?.InitObj();
    }

    public void OnClickNote(int id)
    {
        state?.OnClickHandler(id);
    }
}