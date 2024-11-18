using UnityEngine;

public abstract class TimelineIcon : MonoBehaviour
{
    // 해당 타임라인에 지정된 액션
    private BattleAction _action;
    public BattleAction Action
    {
        protected set { _action = value; }
        get { return _action; }
    }

    public virtual void SetMarking() { }
    public virtual void ClearMarking() { }
    public virtual void UpdateTurnTime() { }
}