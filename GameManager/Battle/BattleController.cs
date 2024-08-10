using UnityEngine;

public class BattleController : MonoBehaviour, IControlState
{
    [Header("참조 스크립트")]
    [SerializeField] private ActionManager actionManager;

    private IControlState subController;

    private void Awake()
    {
        ControlContext.Instance.SetState(this);
    }

    public void SetSubController(IControlState subController)
    {
        this.subController = subController;
    }

    public void OnControlKeyPressed()
    {
        OnUndoKeyPressed();
        OnSubControlKeyPressed();
    }

    public void OnUndoKeyPressed()
    {
        if (Input.GetButtonDown(KeyOption.Cancel))
        {
            actionManager.UndoSelection();
        }
    }

    public void OnSubControlKeyPressed()
    {
        subController?.OnControlKeyPressed();
    }
}