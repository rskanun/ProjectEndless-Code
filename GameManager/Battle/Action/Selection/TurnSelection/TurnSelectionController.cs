using UnityEngine;

public class TurnSelectionController : SelectionController
{
    [Header("컨트롤 스크립트")]
    [SerializeField] private TurnSelection selection;

    public override void OnControlKeyPressed()
    {
        OnUndoKeyPressed();
        OnMoveKeyPressed();
        OnSelectKeyPressed();
    }

    public override void OnUndoKeyPressed()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            selection.UndoSelection();
        }
    }

    public void OnMoveKeyPressed()
    {
        float h = Input.GetAxisRaw("Horizontal");

        if (h > 0f) selection.MoveNext();
        else if (h < 0f) selection.MovePrev();
    }

    public void OnSelectKeyPressed()
    {
        if (Input.GetButtonDown("Select"))
        {
            selection.InsertAction();
        }
    }
}