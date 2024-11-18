using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TalkController : MonoBehaviour, IController
{
    [Header("참조 스크립트")]
    [SerializeField] private TalkManager talkManager;

    // 컨트롤러
    private MainInput.UIActions input;

    private void Awake()
    {
        input = ControlContext.Instance.KeyInput.UI;
    }
    public void OnConnected()
    {
        input.Enable();

        input.Select.performed += OnSelectKeyPressed;
    }

    public void OnDisconnected()
    {
        input.Disable();

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