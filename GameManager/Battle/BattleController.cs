using UnityEngine;
using UnityEngine.InputSystem;

public class BattleController : MonoBehaviour, IControlState
{
    [Header("참조 스크립트")]
    [SerializeField] private ActionManager actionManager;

    // 현재 전황 체크 상태인지
    private bool isSurvey;

    // 각각의 선택창에 따른 추가적인 키조작
    private IControlState subController;

    // 조작키
    private MainInput.BattleActions input;

    private void Awake()
    {
        input = ControlContext.Instance.KeyInput.Battle;

        // 전투 돌입 시 해당 컨트롤러로 전환
        ControlContext.Instance.SetState(this);
    }

    public void SetSubController(IControlState subController)
    {
        // 이전 컨트롤러 비활성화
        this.subController?.OnDisconnected();

        // 다음 컨트롤러 활성화
        this.subController = subController;
        this.subController?.OnConnected();
    }

    public void OnConnected()
    {
        input.Enable();

        input.Cancel.performed += OnCancelKeyPressed;
        input.Survey.performed += OnSurveyKeyPressed;
        input.Parry.performed += OnParryKeyPressed;
    }

    public void OnDisconnected()
    {
        input.Disable();

        input.Cancel.performed -= OnCancelKeyPressed;
        input.Survey.performed -= OnSurveyKeyPressed;
        input.Parry.performed -= OnParryKeyPressed;
    }

    public void OnCancelKeyPressed(InputAction.CallbackContext context)
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

    public void OnSurveyKeyPressed(InputAction.CallbackContext context)
    {
        // 행동 선택창에서만 전황 확인이 가능
        if (subController is ActionSelectionController == false)
        {
            return;
        }

        // 투글 방식으로 전황 체크 키고 끄기
        if (!isSurvey) actionManager.SurveyingBattle();
        else actionManager.ReturnToActionSelect();

        isSurvey = !isSurvey;
    }

    public void OnParryKeyPressed(InputAction.CallbackContext context)
    {
        if (CurrentBattleData.Instance.IsParryEnabled)
        {
            Debug.Log("Parry!!");
        }
    }
}