using UnityEngine;

public class ActionSelectionController : MonoBehaviour, IControlState
{
    [Header("참조 스크립트")]
    [SerializeField] private ActionSelection selection;
    [SerializeField] private ActionManager actionManager;

    // 현재 전황 체크 상태인지
    private bool isChecking;

    public void OnControlKeyPressed()
    {
        OnActionSelectKeyPressed();
    }

    public void OnActionSelectKeyPressed()
    {
        // 누른 키에 따른 행동 선택
        // ex) a키 -> 공격, s키 -> 스킬
    }

    public void OnCheckingKeyPressed()
    {
        if (Input.GetButtonDown(KeyOption.Checking))
        {
            // 투글 방식으로 전황 체크 키고 끄기
            if (!isChecking) actionManager.CheckingAction();
            else actionManager.BackToActionSelect();
        }
    }

    public void OnCancelKeyPressed()
    {
        if (Input.GetButtonDown(KeyOption.Cancel) && isChecking)
        {
            // 전황 체크 상태일 경우 행동 선택창으로 돌아가기
            actionManager.BackToActionSelect();
        }
    }
}