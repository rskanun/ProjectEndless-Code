using UnityEngine;
using UnityEngine.InputSystem;

public class AssistController : MonoBehaviour, IController
{
    [Header("참조 스크립트")]
    [SerializeField] private AssistAttackManager manager;

    public void ControlConnect()
    {
        MainInput.BattleActions input = ControlContext.Instance.KeyInput.Battle;

        input.AssistAttack.performed += OnExtraAttackKeyPressed;
        input.AssistA.performed += OnAssistAKeyPressed;
        input.AssistB.performed += OnAssistBKeyPressed;
        input.AssistC.performed += OnAssistCKeyPressed;
    }

    public void ControlDisconnect()
    {
        MainInput.BattleActions input = ControlContext.Instance.KeyInput.Battle;

        input.AssistAttack.performed -= OnExtraAttackKeyPressed;
        input.AssistA.performed -= OnAssistAKeyPressed;
        input.AssistB.performed -= OnAssistBKeyPressed;
        input.AssistC.performed -= OnAssistCKeyPressed;
    }

    private void OnExtraAttackKeyPressed(InputAction.CallbackContext context)
    {
        // 공격을 막아낸 캐릭터의 지원 공격
        manager.OnAssisAttack();
    }

    private void OnAssistAKeyPressed(InputAction.CallbackContext context)
    {
        manager.OnAssisAttack(0);
    }

    private void OnAssistBKeyPressed(InputAction.CallbackContext context)
    {
        manager.OnAssisAttack(1);
    }

    private void OnAssistCKeyPressed(InputAction.CallbackContext context)
    {
        manager.OnAssisAttack(2);
    }
}