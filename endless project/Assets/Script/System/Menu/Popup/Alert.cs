using Assets.Script.System.Menu.Popup;
using Assets.Script.UI.Menu.Popup;
using DG.Tweening;
using System.Collections;
using UnityEngine;

public class Alert : Popup
{
    private GameObject alertObj;
    private AlertUI ui;

    public Alert()
    {
        alertObj = AlertManager.Instance.Alert;
        ui = alertObj.GetComponent<AlertUI>();
    }

    public static Alert makeMsg(string msg, string okText = "확인")
    {
        Alert alert = new Alert();
        alert.ui.setAlert(msg, okText);

        alert.setOkCallBack(null);

        return alert;
    }

    public void setOkCallBack(AlertUI.PopupCallBack listener)
    {
        listener += () => PopupManager.Instance.destroyPopup(this);
        ui.setOkCallBack(listener);
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
}