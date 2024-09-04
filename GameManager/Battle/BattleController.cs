using UnityEngine;

public class BattleController : MonoBehaviour, IControlState
{
    [Header("참조 스크립트")]
    [SerializeField] private ActionManager actionManager;

    // 현재 전황 체크 상태인지
    private bool isSurvey;

    // 각각의 선택창에 따른 추가적인 키조작
    private IControlState subController;

    private void Awake()
    {
        ControlContext.Instance.SetState(this);
    }

    public void SetSubController(IControlState subController)
    {
        this.subController = subController;
    }

    public void OnControlKeyPressed()
    {
        OnSurveyKeyPressed();
        OnCancelKeyPressed();
        OnSubControlKeyPressed();
    }

    public void OnCancelKeyPressed()
    {
        if (Input.GetButtonDown(KeyOption.Cancel))
        {
            // 전황 체크 상태일 경우 행동 선택창으로 돌아가기
            if (isSurvey)
            {
                actionManager.ReturnToActionSelect();
                isSurvey = false;
            }
            // 그 외엔 선택창 되돌리기
            else actionManager.UndoSelection();
        }
    }

    public void OnSurveyKeyPressed()
    {
        // 행동 선택창에서만 전황 확인이 가능
        if (subController is ActionSelectionController == false)
        {
            return;
        }

        if (Input.GetButtonDown(KeyOption.Survey))
        {
            // 투글 방식으로 전황 체크 키고 끄기
            if (!isSurvey) actionManager.SurveyingBattle();
            else actionManager.ReturnToActionSelect();

            isSurvey = !isSurvey;
        }
    }

    public void OnSubControlKeyPressed()
    {
        subController?.OnControlKeyPressed();
    }
}