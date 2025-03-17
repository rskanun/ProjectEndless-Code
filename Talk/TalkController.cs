using UnityEngine;
using UnityEngine.InputSystem;

public class TalkController : MonoBehaviour, IController
{
    [Header("참조 스크립트")]
    [SerializeField] private TalkManager talkManager;

    private void Awake()
    {
        ControlContext.Instance.RegisterController(this);
    }

    private void OnDestroy()
    {
        ControlContext.Instance.RemoveController(this);
    }

    public void ControlConnect()
    {
        MainInput.UIActions input = ControlContext.Instance.KeyInput.UI;

        input.Select.performed += OnSelectKeyPressed;
    }

    public void ControlDisconnect()
    {
        MainInput.UIActions input = ControlContext.Instance.KeyInput.UI;

        input.Select.performed -= OnSelectKeyPressed;
    }

    /************************************************************
    * [대화키]
    * 
    * 대사를 읽어 그에 따른 인게임 이벤트 제어
    ************************************************************/

    private void OnSelectKeyPressed(InputAction.CallbackContext context)
    {
        talkManager.OnTalkHandler();
    }
}