using UnityEngine;
using UnityEngine.InputSystem;

public class AssistController : MonoBehaviour, IController
{
    [Header("참조 스크립트")]
    [SerializeField] private AssistAttackManager manager;

    private MainInput.BattleActions input;

    private void Awake()
    {
        input = ControlContext.Instance.KeyInput.Battle;
    }

    public void OnConnected()
    {
        input.Enable();

        input.AssistAttack.performed += OnAssistAttackKeyPressed;
        input.AssistA.performed += OnAssistAttackKeyPressed;
        input.AssistB.performed += OnAssistAttackKeyPressed;
        input.AssistC.performed += OnAssistAttackKeyPressed;
    }

    public void OnDisconnected()
    {
        input.Disable();

        input.AssistAttack.performed -= OnAssistAttackKeyPressed;
        input.AssistA.performed -= OnAssistAttackKeyPressed;
        input.AssistB.performed -= OnAssistAttackKeyPressed;
        input.AssistC.performed -= OnAssistAttackKeyPressed;
    }

    public void OnAssistAttackKeyPressed(InputAction.CallbackContext context)
    {
        string pressKey = context.action.name;

        if (pressKey.Equals(input.AssistAttack.name))
        {
            // 플레이어의 지원 공격
            manager.OnAssisAttack(0);
        }
        else if (pressKey.Equals(input.AssistA.name))
        {
            manager.OnAssisAttack(1);
        }
    }
}