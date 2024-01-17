using UnityEngine;

public class LoadState : MonoBehaviour, INoteState
{
    [Header("참조 오브젝트")]
    [SerializeField] private NoteManager manager;
    [SerializeField] private NoteUI ui;

    public void InitObj()
    {
        ui.InitNotice();
    }

    public void OnClickHandler(int id)
    {
        manager.LoadHandler(id);
    }
}