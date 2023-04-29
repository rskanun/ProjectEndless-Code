using Assets.Script.System.Menu.Popup;
using Assets.Script.UI.Menu.Popup;
using DG.Tweening;
using UnityEngine;

public class Confirm : Popup
{
    private GameObject confirmObj;
    private ConfirmUI ui;

    public Confirm(string msg, string yesText, string noText)
    {
        confirmObj = ConfirmManager.Instance.Confirm;
        ui = confirmObj.GetComponent<ConfirmUI>();

        ui.setConfirm(msg, yesText, noText);

        setNoCallBack(null);
        setYesCallBack(null);
    }

    public static Confirm makeMsg(string msg, string yesText = "네", string noText = "아니요")
    {
        return new Confirm(msg, yesText, noText);
    }

    public Confirm setYesCallBack(ConfirmUI.PopupCallBack listener)
    {
        listener += () => PopupManager.Instance.popupDestroy(this); ;
        ui.setYesCallBack(listener);

        return this;
    }

    public Confirm setNoCallBack(ConfirmUI.PopupCallBack listener)
    {
        listener += () => PopupManager.Instance.popupDestroy(this);
        ui.setNoCallBack(listener);

        return this;
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

    public override void close()
    {
        ui.onNo();
    }
}
