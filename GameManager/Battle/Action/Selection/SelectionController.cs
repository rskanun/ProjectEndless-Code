using UnityEngine;

public abstract class SelectionController : MonoBehaviour, IControlState
{
    public void ActiveController()
    {
        ControlContext.Instance.SetState(this);
        Debug.Log($"Active {GetType().Name}");
    }

    public void DeactiveController()
    {
        ControlContext.Instance.SetState(null);
        Debug.Log($"Deactive {GetType().Name}");
    }

    public abstract void OnControlKeyPressed();
    public abstract void OnUndoKeyPressed();
}