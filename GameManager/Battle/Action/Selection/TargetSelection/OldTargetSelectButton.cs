using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OldTargetSelectButton : MonoBehaviour
{
    public Button button;

    private System.Action onClickAction;
    private bool isLastHover;

    // 선택 타겟
    private Entity selectTarget;
    private bool IsEnemy => selectTarget is Monster;
    private bool IsFront => selectTarget.Position == BattlePosition.Front;
    private bool IsSelectable => !selectTarget.IsDead;

    public void OnHover()
    {
        if (button.interactable)
        {
            isLastHover = true;
            EventSystem.current.SetSelectedGameObject(button.gameObject);
        }
    }

    public void OnClick()
    {
        onClickAction?.Invoke();
    }

    private void Update()
    {
        if (isLastHover)
        {
            // 버튼 선택이 해제되었을 경우 이전 버튼 선택
            if (EventSystem.current.currentSelectedGameObject == null)
            {
                EventSystem.current.SetSelectedGameObject(gameObject);
            }

            // 최근에 선택된 버튼이 아닌 경우 해제
            if (EventSystem.current.currentSelectedGameObject != gameObject)
            {
                isLastHover = false;
            }
        }
    }

    /***************************************************************
    * [ 버튼 설정 ]
    * 
    * 해당 타겟 선택 버튼 설정
    ***************************************************************/

    public void SetTarget(Entity target)
    {
        selectTarget = target;
    }

    public void SetListener(System.Action listener)
    {
        onClickAction = listener;
    }

    /***************************************************************
    * [ 버튼 활성화 ]
    * 
    * 자신과 조건에 맞는 
    ***************************************************************/

    public void EnemyFrontActive()
    {
        SetActive(IsEnemy && IsFront);
    }

    public void EnemyActive()
    {
        SetActive(IsEnemy);
    }

    public void PlayerPartyActive()
    {
        SetActive(!IsEnemy);
    }

    public void Deactive()
    {
        SetActive(false);
    }

    private void SetActive(bool isActive)
    {
        button.interactable = isActive && selectTarget.IsDead == false;
    }
}