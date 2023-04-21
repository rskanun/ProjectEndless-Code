using Assets.Script.System.Menu.Popup;
using Assets.Script.UI.Menu.Popup;
using System.Collections;
using UnityEngine;

public class Alert
{
    private GameObject _alertObj;
    private AlertUI _ui;

    public Alert()
    {
        _alertObj = AlertManager.Instance.Alert;
        _ui = _alertObj.GetComponent<AlertUI>();
    }

    public static Alert makeMsg(string msg, string okText = "확인")
    {
        Alert alert = new Alert();
        alert._ui.setAlert(msg, okText);

        return alert;
    }

    public void show()
    {
        _ui.setActive(true);
    }
}