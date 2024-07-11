using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Selection Manager", fileName = "SelectionManager")]
public class SelectionManager : ScriptableObject
{
    [Header("선택 시 발동할 이벤트")]
    [SerializeField] private GameEvent selectEvent;

    private List<SelectableTarget> listeners = new List<SelectableTarget>();
    private SelectableTarget hoverTarget;
    private GameObject _selectTarget;
    public GameObject SelectTarget
    {
        private set { _selectTarget = value; }
        get { return _selectTarget; }
    }

    public void RegisterListener(SelectableTarget listener)
    {
        listeners.Add(listener);
    }

    public void RemoveListener(SelectableTarget listener)
    {
        listeners.Remove(listener);
    }

    public void NotifySelectableFront()
    {
        if (BattleData.Instance.EnemyFrontCount <= 0)
        {
            // 전위가 모두 사망한 경우 후위도 공격 가능
            NotifySelectableEnemy();

            return;
        }

        // 적 파티의 전위를 타겟으로 선택가능하게 설정
        SelectableTarget firstTarget = null;

        foreach (SelectableTarget target in listeners)
        {
            bool isSelectable = target.isEnemy && target.isFront;

            if (isSelectable && firstTarget == null)
            {
                // 자동으로 선택해놓을 대상 선택
                firstTarget = target;
            }

            target.SetSelectable(isSelectable);
        }

        // 첫번째 대상 자동 선택
        HoverTarget(firstTarget);
    }

    public void NotifySelectableEnemy()
    {
        // 적 파티를 타겟으로 선택가능하게 설정
        SelectableTarget firstTarget = null;

        foreach (SelectableTarget target in listeners)
        {
            bool isSelectable = target.isEnemy;

            if (isSelectable && firstTarget == null)
            {
                // 자동으로 선택해놓을 대상 선택
                firstTarget = target;
            }

            target.SetSelectable(isSelectable);
        }

        // 첫번째 대상 자동 선택
        HoverTarget(firstTarget);
    }

    public void NotifySelectableMember()
    {
        foreach (SelectableTarget target in listeners)
        {
            bool isSelectable = !target.isEnemy;
            target.SetSelectable(isSelectable);
        }
    }

    public void HoverTarget(SelectableTarget target)
    {
        if (hoverTarget != null)
        {
            // 이전 대상의 표식 지우기
            hoverTarget.SelectCancel();
        }

        // 현재 선택된 대상 바꾸기
        hoverTarget = target;
        hoverTarget.SelectThis();
    }

    public void OnSelect()
    {
        if (hoverTarget != null)
        {
            SelectTarget = hoverTarget.gameObject;

            // 선택 표식 초기화
            hoverTarget.SelectCancel();
            foreach (SelectableTarget target in listeners)
            {
                target.SetSelectable(false);
            }

            // 선택 알림 보내기
            selectEvent.NotifyUpdate();
        }
    }
}