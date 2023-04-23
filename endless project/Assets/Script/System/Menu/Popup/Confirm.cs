using Assets.Script.System.Menu.Popup;
using Assets.Script.UI.Menu.Popup;
using DG.Tweening;
using UnityEngine;

public class Confirm : Popup
{
    private GameObject confirmObj;
    private ConfirmUI ui;

    public Confirm()
    {
        confirmObj = ConfirmManager.Instance.Confirm;
        ui = confirmObj.GetComponent<ConfirmUI>();
    }

    public static Confirm makeMsg(string msg, string yesText = "네", string noText = "아니요.")
    {
        Confirm confirm = new Confirm();

        confirm.ui.setConfirm(msg, yesText, noText);

        confirm.setNoCallBack(null);
        confirm.setYesCallBack(null);

        return confirm;
    }

    public void setYesCallBack(ConfirmUI.PopupCallBack listener)
    {
        listener += () => PopupManager.Instance.destroyPopup(this); ;
        ui.setYesCallBack(listener);
    }

    public void setNoCallBack(ConfirmUI.PopupCallBack listener)
    {
        listener += () => PopupManager.Instance.destroyPopup(this);
        ui.setNoCallBack(listener);
    }

    public override void show()
    {
        ui.show();
    }

    public override void destroy()
    {
        ui.hide()
            .OnComplete(() => Object.Destroy(confirmObj));
    }
}
