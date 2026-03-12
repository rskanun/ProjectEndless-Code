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

        ui.SetConfirm(msg, yesText, noText);

        SetNoHandler(null);
        SetYesHandler(null);
    }

    public static Confirm CreateMsg(string msg, string yesText = "네", string noText = "아니요")
    {
        return new Confirm(msg, yesText, noText);
    }

    public Confirm SetYesHandler(ConfirmUI.PopupCallBack listener)
    {
        listener += () => PopupManager.Instance.Destroy(this);
        ui.SetYesCallBack(listener);

        return this;
    }

    public Confirm SetNoHandler(ConfirmUI.PopupCallBack listener)
    {
        listener += () => PopupManager.Instance.Destroy(this);
        ui.SetNoCallBack(listener);

        return this;
    }

    public override void Show()
    {
        ui.Show();
    }

    public override void Destroy()
    {
        ui.Hide()
            .OnComplete(() => Object.Destroy(confirmObj));
    }

    public override void Close()
    {
        ui.OnClickNo();
    }
}
