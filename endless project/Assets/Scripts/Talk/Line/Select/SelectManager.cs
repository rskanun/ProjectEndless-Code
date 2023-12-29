using System;
using UnityEngine;

public class SelectManager : MonoBehaviour
{
    [Header("참조 스크립트")]
    [SerializeField] private SelectUI ui;

    private Action<string> onClickHandler;

    public void OpenSelect(Select select, Action<string> onClickHandler)
    {
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
    }
}