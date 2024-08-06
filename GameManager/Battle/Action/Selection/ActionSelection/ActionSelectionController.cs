using UnityEngine;

public class ActionSelectionController : SelectionController
{
    // 컨트롤 스크립트
    [SerializeField] private ActionSelection selection;

    public override void OnSelectionControlKeyPressed()
    {
        OnActionSelectKeyPressed();
    }

    public void OnActionSelectKeyPressed()
    {
        // 누른 키에 따른 행동 선택
        // ex) a키 -> 공격, s키 -> 스킬
    }
}