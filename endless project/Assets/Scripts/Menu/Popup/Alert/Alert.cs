using DG.Tweening;
using UnityEngine;

public class Alert : Popup
{
    private GameObject alertObj;
    private AlertUI ui;

    private bool _isActive;
    public bool isActive { get { return _isActive; } }

    public Alert(string msg, string okText)
    {
        alertObj = AlertManager.Instance.Alert;
        ui = alertObj.GetComponent<AlertUI>();

        ui.SetAlert(msg, okText);

        SetOkHandler(null);
    }

    public static Alert CreateMsg(string msg, string okText = "확인")
    {
        return new Alert(msg, okText);
    }

    public Alert SetOkHandler(AlertUI.PopupCallBack listener)
    {
        listener += () => PopupManager.Instance.Destroy(this);
        ui.SetOkCallBack(listener);

        return this;
    }

    public override void Show()
    {
        _isActive = true;

        ui.Show();
    }

    public override void Destroy()
    {
        ui.Hide()
            .OnComplete(() => Object.Destroy(alertObj));
    }

    public override void Close()
    {
        ui.OnClick();

        _isActive = false;
    }
}