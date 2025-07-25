using UnityEngine;
using UnityEngine.InputSystem;

public class TeamContactController : MonoBehaviour, IController
{
    [SerializeField] private TeamContactWindow contactWindow;

    public void ControlConnect()
    {
        MainInput.MenuActions menuInput = ControlContext.Instance.KeyInput.Menu;

        menuInput.Context.performed += OnJoinPartyKeyPressed;
    }

    public void ControlDisconnect()
    {
        MainInput.MenuActions menuInput = ControlContext.Instance.KeyInput.Menu;

        menuInput.Context.performed -= OnJoinPartyKeyPressed;
    }

    private void OnJoinPartyKeyPressed(InputAction.CallbackContext context)
    {
        contactWindow.SwitchPartyState();
    }
}