using Assets.Script.System.Menu.Popup;
using Assets.Script.UI.Menu.Popup;
using UnityEngine;

public class Confirm
{
    private GameObject _confirmObj;
    private ConfirmUI _ui;

    public Confirm()
    {
        _confirmObj = ConfirmManager.Instance.Confirm;
        _ui = _confirmObj.GetComponent<ConfirmUI>();
    }

    public static Confirm makeMsg(string msg, string yesText = "네", string noText = "아니요.")
    {
        Confirm confirm = new Confirm();
        confirm._ui.setConfirm(msg, yesText, noText);

        return confirm;
    }

    public void setYesCallBack(ConfirmUI.PopupCallBack listener)
    {
        _ui.setYesCallBack(listener);
    }

    public void setNoCallBack(ConfirmUI.PopupCallBack listener)
    {
        _ui.setNoCallBack(listener);
    }

    public void show()
    {
        _ui.setActive(true);
    }
}
