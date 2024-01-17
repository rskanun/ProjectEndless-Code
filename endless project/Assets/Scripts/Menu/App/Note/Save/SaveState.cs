using UnityEngine;

public class SaveState : MonoBehaviour, INoteState
{
    [Header("참조 오브젝트")]
    [SerializeField] private NoteManager manager;
    [SerializeField] private NoteUI ui;

    public void InitObj()
    {
        ui.UpdateAddSaveButton();
    }

    public void OnClickHandler(int id)
    {
        manager.SaveHandler(id);
    }
}