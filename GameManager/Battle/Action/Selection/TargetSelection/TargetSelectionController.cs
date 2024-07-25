using UnityEngine;

public class TargetSelectionController : SelectionController
{
    [Header("컨트롤 스크립트")]
    [SerializeField] private TargetSelection selection;

    public override void OnControlKeyPressed()
    {
        OnUndoKeyPressed();
        OnNavigateKeyPressed();
        OnSelectKeyPressed();
    }

    public override void OnUndoKeyPressed()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            selection.UndoSelection();
        }
    }

    public void OnNavigateKeyPressed()
    {
        float h = Input.GetAxisRaw("Horizontal");

        // 0보다 작으면 이전 대상으로 이동
        // 0보다 크면 다음 대상으로 이동
    }

    public void OnSelectKeyPressed()
    {
        if (Input.GetButtonDown("Select"))
        {
            // 현재 대상이 된 타겟 선택
        }
    }
}