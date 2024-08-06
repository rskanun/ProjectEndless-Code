using UnityEngine;

public class SelectionController : MonoBehaviour, IControlState
{
    [Header("컨트롤 스크립트")]
    [SerializeField] private ActionManager actionManager;

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

    public void OnControlKeyPressed()
    {
        OnUndoKeyPressed();
        OnSelectionControlKeyPressed();
    }

    public void OnUndoKeyPressed()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            actionManager.UndoSelection();
        }
    }

    public virtual void OnSelectionControlKeyPressed() { }
}