public class TalkContext
{
    private static TalkContext _instance;
    public static TalkContext Instance
    {
        get
        {
            if (_instance == null)
                _instance = new TalkContext();

            return _instance;
        }
    }

    private TalkManager manager;

    public void RegisterManager(TalkManager manager)
    {
        this.manager = manager;
    }

    public void RemoveManager()
    {
        manager = null;
    }

    public void ActiveDialogue(Npc npc)
    {
        manager.StartTalk(npc);
    }
}