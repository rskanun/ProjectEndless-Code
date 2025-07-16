using System;
using DG.Tweening;
using UnityEngine;

public class OptionUI : AppUI
{
    [Header("참조 오브젝트")]
    [SerializeField] private GameObject cancelPanel;

    protected override void ActiveAppWithAnimation(Action openHandler)
    {
        MenuAnimation.AppToastOpenAnimation(window, appBackground, openHandler)
            .AppendCallback(() => _isOpened = true);
    }

    protected override void DeactiveAppWithAnimation()
    {
        _isOpened = false;

        MenuAnimation.AppCloseAnimation(window, appBackground);
    }

    public void SetCancelPanel(bool isVeiw)
    {
        cancelPanel.SetActive(isVeiw);
    }
}