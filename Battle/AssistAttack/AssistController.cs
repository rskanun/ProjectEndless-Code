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

        input.AssistAttack.performed += OnExtraAttackKeyPressed;
        input.AssistA.performed += ctx => OnAssistAttackKeyPressed(0);
        input.AssistB.performed += ctx => OnAssistAttackKeyPressed(1);
        input.AssistC.performed += ctx => OnAssistAttackKeyPressed(2);
    }

    public void OnDisconnected()
    {
        input.Disable();

        input.AssistAttack.performed -= OnExtraAttackKeyPressed;
        input.AssistA.performed -= ctx => OnAssistAttackKeyPressed(0);
        input.AssistB.performed -= ctx => OnAssistAttackKeyPressed(1);
        input.AssistC.performed -= ctx => OnAssistAttackKeyPressed(2);
    }

    public void OnExtraAttackKeyPressed(InputAction.CallbackContext context)
    {
        // 공격을 막아낸 캐릭터의 지원 공격
        manager.OnAssisAttack();
    }

    public void OnAssistAttackKeyPressed(int index)
    {
        // 지원 가능한 index번째 캐릭터에게 지원 호출
        manager.OnAssisAttack(index);
    }
}