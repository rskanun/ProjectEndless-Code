using System;
using UnityEngine;

public class SelectManager : MonoBehaviour
{
    [Header("참조 스크립트")]
    [SerializeField] private SelectUI ui;

    private Action<string> onClickHandler;

    private bool isSelectOpen;
    public bool IsSelectOpen
    {
        get { return isSelectOpen; }
    }

    public void OpenSelect(Select select, Action<string> onClickHandler)
    {
        isSelectOpen = true;

        this.onClickHandler = onClickHandler;

        string[] options = select.Options.ToArray();

        ui.createButtons(options, OnButtonClick);
        ui.setView(true);
    }

    private void OnButtonClick(string option)
    {
        onClickHandler(option);
        CloseSelect();
    }

    public void CloseSelect()
    {
        ui.destroySelect();
        ui.setView(false);

        isSelectOpen = false;
    }
}