using System;
using DG.Tweening;
using UnityEngine;

public class OptionUI : AppUI
{
    [Header("참조 오브젝트")]
    [SerializeField] private GameObject cancelPanel;

    public override void OpenApp(Action openHandler)
    {
        MenuAnimation.AppToastOpenAnimation(window, appBackground)
            .AppendCallback(() => openHandler?.Invoke());
    }

    public override void CloseApp(Action closeHandler)
    {
        MenuAnimation.AppCloseAnimation(window, appBackground)
            .AppendCallback(() => closeHandler?.Invoke());
    }

    public void SetCancelPanel(bool isVeiw)
    {
        cancelPanel.SetActive(isVeiw);
    }
}