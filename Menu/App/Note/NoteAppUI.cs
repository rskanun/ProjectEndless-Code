using DG.Tweening;

public class NoteAppUI : AppUI
{
    protected override Sequence AppCloseAnimation(bool isPlayAnimation)
    {
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