using UnityEngine;

public class NoteApp : App
{
    [Header("노트앱 모드")]
    [SerializeField] private INoteState state;

    [Header("참조 스크립트")]
    [SerializeField] private NoteManager manager;

    protected override void LoadData()
    {
        manager.InitSaveFile();
        state.InitObj();
    }

    public void OnClickNote(int id)
    {
        state.OnClickHandler(id);
    }
}