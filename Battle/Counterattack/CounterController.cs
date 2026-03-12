using UnityEngine;
using UnityEngine.InputSystem;

public class CounterController : MonoBehaviour, IController
{
    [Header("참조 스크립트")]
    [SerializeField] private CounterattackSelection manager;

    public void ControlConnect()
    {
        MainInput.BattleActions input = ControlContext.Instance.KeyInput.Battle;

        input.AssistAttack.performed += OnExtraAttackKeyPressed;
        input.AssistA.performed += OnAssistAKeyPressed;
        input.AssistB.performed += OnAssistBKeyPressed;
    }

    public void ControlDisconnect()
    {
        MainInput.BattleActions input = ControlContext.Instance.KeyInput.Battle;

        input.AssistAttack.performed -= OnExtraAttackKeyPressed;
        input.AssistA.performed -= OnAssistAKeyPressed;
        input.AssistB.performed -= OnAssistBKeyPressed;
    }

    private void OnExtraAttackKeyPressed(InputAction.CallbackContext context)
    {
        // 공격을 막아낸 캐릭터의 지원 공격
        manager.SelectAttacker(0);
    }

    private void OnAssistAKeyPressed(InputAction.CallbackContext context)
    {
        manager.SelectAttacker(1);
    }

    private void OnAssistBKeyPressed(InputAction.CallbackContext context)
    {
        manager.SelectAttacker(2);
    }
}