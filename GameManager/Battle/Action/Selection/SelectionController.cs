using UnityEngine;

public abstract class SelectionController : MonoBehaviour, IControlState
{
    private void OnEnable()
    {
        ControlContext.Instance.SetState(this);
    }

    private void OnDisable()
    {
        ControlContext.Instance.SetState(null);
    }

    public abstract void OnControlKeyPressed();
    public abstract void OnUndoKeyPressed();
}