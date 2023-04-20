using Assets.Script.UI.Menu.Popup;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Confirm : MonoBehaviour
{
    [SerializeField] private GameObject confirmPrefab;
    [SerializeField] private Transform confirmParent;

    private List<GameObject> confirmList = new List<GameObject>();

    public void makeMsg(string msg, string yesText = "네", string noText = "아니요")
    {
        GameObject confirm = Instantiate(confirmPrefab);

        ConfirmUI ui = confirm.GetComponent<ConfirmUI>();
        ui.setContents(msg);
        ui.setYesText(yesText);
        ui.setNoText(noText);

        confirmList.Add(confirm);
    }
}
