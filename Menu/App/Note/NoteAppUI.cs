using DG.Tweening;
using UnityEngine;

public class NoteAppUI : AppUI
{
    [SerializeField] private GameObject window;

    [Header("참조 오브젝트")]
    [SerializeField] private GameObject appBackground;

    [Header("참조 스크립트")]
    [SerializeField] private HomeScreenUI homeScreenUI;

    protected override Sequence AppCloseAnimation(bool isPlayAnimation)
    {
        homeScreenUI.EnabledHomeScreen(isPlayAnimation);

        if (!isPlayAnimation)
        {
            // 애니메이션 스킵
            window.SetActive(false);
            appBackground.SetActive(false);

            return DOTween.Sequence();
        }

        return MenuAnimation.AppCloseAnimation(window, appBackground);
    }

    protected override Sequence AppOpenAnimation(bool isPlayAnimation)
    {
        homeScreenUI.DisabledHomeScreen(isPlayAnimation);

        if (!isPlayAnimation)
        {
            // 애니메이션 스킵
            window.SetActive(true);
            appBackground.SetActive(true);

            return DOTween.Sequence();
        }

        return MenuAnimation.AppOpenAnimation(window, appBackground);
    }
}