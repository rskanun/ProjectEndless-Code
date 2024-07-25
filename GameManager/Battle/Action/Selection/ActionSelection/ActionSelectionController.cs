using UnityEngine;

public class ActionSelectionController : SelectionController
{
    [Header("컨트롤 스크립트")]
    [SerializeField] private ActionSelection selection;

    public override void OnControlKeyPressed()
    {
        OnUndoKeyPressed();
    }

    public override void OnUndoKeyPressed()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            selection.UndoSelection();
        }
    }

    public void OnActionSelectKeyPressed()
    {
        // 누른 키에 따른 행동 선택
        // ex) a키 -> 공격, s키 -> 스킬
    }
}