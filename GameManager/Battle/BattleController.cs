using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour, IControlState
{
    [Header("참조 스크립트")]
    [SerializeField] private ActionManager actionManager;

    /***************************************************************
    * [ 전투 조작 ]
    * 
    * 플레이어의 입력에 따른 행동, 타겟, 턴 선택 등을 처리
    ***************************************************************/

    public void OnControlKeyPressed()
    {
        OnActionSelectKeyPressed();
        OnCancelKeyPressed();
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