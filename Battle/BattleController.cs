using UnityEngine;
using UnityEngine.InputSystem;

public class BattleController : MonoBehaviour, IController
{
    [Header("참조 스크립트")]
    [SerializeField] private ActionManager actionManager;

    // 참조 데이터
    private BattleData battleData
        => BattleData.Instance;

    // 현재 전황 체크 상태인지
    private bool isSurvey;

    // 각각의 선택창에 따른 추가적인 키조작
    private IController subController;

    private void OnEnable()
    {
        ControlContext.Instance.EnableController(this);
    }

    private void OnDisable()
    {
        ControlContext.Instance.DisableController(this);
    }

    private void OnDestroy()
    {
        ControlContext.Instance.RemoveController(this);
    }

    public void SetSubController(IController subController)
    {
        // 이전 컨트롤러 비활성화
        this.subController?.ControlDisconnect();

        // 다음 컨트롤러 활성화
        this.subController = subController;
        this.subController?.ControlConnect();
    }

    public void ControlConnect()
    {
        // Connect Battle Input
        MainInput.BattleActions battleInput = ControlContext.Instance.KeyInput.Battle;
        battleInput.Survey.performed += OnSurveyKeyPressed;
        battleInput.Parry.performed += OnParryKeyPressed;
        battleInput.Dodge.performed += OnDodgeKeyPressed;

        // Connect UI Input
        MainInput.UIActions uiInput = ControlContext.Instance.KeyInput.UI;
        uiInput.Cancel.performed += OnCancelKeyPressed;
    }

    public void ControlDisconnect()
    {
        // Disable Battle Input
        MainInput.BattleActions battleInput = ControlContext.Instance.KeyInput.Battle;
        battleInput.Survey.performed -= OnSurveyKeyPressed;
        battleInput.Parry.performed -= OnParryKeyPressed;
        battleInput.Dodge.performed -= OnDodgeKeyPressed;

        // Disable UI Input
        MainInput.UIActions uiInput = ControlContext.Instance.KeyInput.UI;
        uiInput.Cancel.performed -= OnCancelKeyPressed;
    }

    private void OnCancelKeyPressed(InputAction.CallbackContext context)
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

    private void OnSurveyKeyPressed(InputAction.CallbackContext context)
    {
        // 행동 선택창에서만 전황 확인이 가능
        if ((subController is ActionSelectionController or SurveyController) == false)
        {
            return;
        }

        // 투글 방식으로 전황 체크 키고 끄기
        if (!isSurvey) actionManager.SurveyingBattle();
        else actionManager.ReturnToActionSelect();

        isSurvey = !isSurvey;
    }

    private void OnParryKeyPressed(InputAction.CallbackContext context)
    {
        // 패링을 사용할 수 있는 경우에만 사용
        if (battleData.IsUsedParry)
        {
            // 공격 방어 기능을 한 번 사용하면 다른 기능 사용 X
            DisableDefensive();

            if (battleData.IsParryFrame)
            {
                BattleAction curAction = battleData.Sequence.GetTurnAction(0);
                Entity defender = curAction.GetTargets()[0]; // 타겟 중 한 명이 대표로 패링

                curAction.actor.OnParried();
                defender.OnParrying(curAction.actor);
            }
        }
    }

    private void OnDodgeKeyPressed(InputAction.CallbackContext context)
    {
        if (battleData.IsUsedDodge)
        {
            Debug.Log("Dodge Action");
            // 공격 방어 기능을 한 번 사용하면 다른 기능 사용 X
            DisableDefensive();

            // 회피 타이밍에 누른 경우
            if (battleData.IsDodgeFrame)
            {
                // 다음 회피 기회 주기
                battleData.ExtraDodgeCount = true;

                // 현재 공격에 대한 회피 처리
                BattleAction curAction = battleData.Sequence.GetTurnAction(0);
                foreach (Entity target in curAction.GetTargets())
                {
                    // 현재 타겟이 된 모든 캐릭터들이 공격 회피
                    target.OnDodge();
                }
            }
        }
    }

    private void DisableDefensive()
    {
        battleData.IsUsedParry = false;
        battleData.IsUsedDodge = false;
    }
}