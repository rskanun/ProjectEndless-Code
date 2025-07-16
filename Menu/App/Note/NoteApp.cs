using UnityEngine;

public class NoteApp : App
{
    [Header("노트 앱 참조 스크립트")]

    [SerializeField]
    private NoteManager _manager;
    protected NoteManager Manager
    {
        get { return _manager; }
    }

    [SerializeField]
    private NoteUI _ui;
    protected NoteUI UI
    {
        get { return _ui; }
    }

    protected override void OnOpen()
    {
        _manager.InitSaveFile();

        NoteContext.Instance.InitAdditionalObj();
    }
}