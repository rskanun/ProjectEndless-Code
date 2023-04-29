using Assets.Script.System.Menu.Popup;
using Assets.Script.UI.Menu.Popup;
using DG.Tweening;
using System.Collections;
using UnityEngine;

public class Alert : Popup
{
    private GameObject alertObj;
    private AlertUI ui;

    public Alert(string msg, string okText)
    {
        alertObj = AlertManager.Instance.Alert;
        ui = alertObj.GetComponent<AlertUI>();

        ui.setAlert(msg, okText);

        setOkCallBack(null);
    }

    public static Alert makeMsg(string msg, string okText = "확인")
    {
        return new Alert(msg, okText);
    }

    public Alert setOkCallBack(AlertUI.PopupCallBack listener)
    {
        listener += () => PopupManager.Instance.popupDestroy(this);
        ui.setOkCallBack(listener);

        return this;
    }

    public override void show()
    {
        ui.show();
    }

    public override void destroy()
    {
        ui.hide()
            .OnComplete(() => Object.Destroy(alertObj));
    }

    public override void close()
    {
        ui.onClick();
    }
}