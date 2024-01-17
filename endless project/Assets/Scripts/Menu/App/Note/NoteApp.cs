using UnityEngine;

public class NoteApp : App
{
    [Header("노트 앱 참조 스크립트")]

    [SerializeField]
    private NoteManager _manager;
    public NoteManager manager
    {
        get { return _manager; }
    }

    [SerializeField]
    private NoteUI _ui;
    public NoteUI ui
    {
        get { return _ui; }
    }

    protected override void LoadData()
    {
        _manager.InitSaveFile();

        NoteContext.Instance.InitAdditionalObj();
    }
}