using UnityEngine;

public class TurnSelectionController : SelectionController
{
    // 컨트롤 스크립트
    [SerializeField] private TurnSelection selection;

    public override void OnSelectionControlKeyPressed()
    {
        // 턴 선택 키
        OnMoveKeyPressed();
        OnSelectKeyPressed();
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