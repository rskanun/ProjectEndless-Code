using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour, IControlState
{
    [Header("참조 스크립트")]
    [SerializeField] private ActionManager actionManager;

    private void Awake()
    {
        ControlContext.Instance.SetState(this);
    }

    private void Update()
    {
        // TMP GameManager Function
        ControlContext.Instance.OnKeyPressed();
    }

    /***************************************************************
    * [ 전투 조작 ]
    * 
    * 플레이어의 입력에 따른 행동, 타겟, 턴 선택 등을 처리
    ***************************************************************/

    public void OnControlKeyPressed()
    {
        OnTimelineMoveKeyPressed();
        OnActionSelectKeyPressed();
        OnCancelKeyPressed();
    }

    public void OnTimelineMoveKeyPressed()
    {

    }

    private void OnActionSelectKeyPressed()
    {

    }

    private void OnCancelKeyPressed()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            actionManager.UndoSelection();
        }
    }
}