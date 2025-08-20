using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;

public class MenuAnimation
{
    // 메뉴 열리고 닫히는 각도

    /************************************************************
    * [메뉴 애니메이션]
    * 
    * 메뉴와 관련된 애니메이션
    ************************************************************/

    public static Sequence HomeScreenShowAnimation(GameObject homeScreen)
    {
        return BiggerSeq(homeScreen, 0.5f, 1f, 0.15f);
    }

    public static Sequence HomeScreenHideAnimation(GameObject homeScreen)
    {
        return SmallerSeq(homeScreen, 1f, 0.5f, 0.15f);
    }

    /************************************************************
    * [앱 애니메이션]
    * 
    * 앱과 관련된 애니메이션
    ************************************************************/

    public static Sequence AppToastOpenAnimation(GameObject window, GameObject background)
    {
        return BiggerOpenSeq(background, 0.1f, 0.08f)
                .AppendInterval(0.12f)
                .Append(ToastWindowSeq(window, 0.2f));
    }

    public static Sequence AppCloseAnimation(GameObject window, GameObject background)
    {
        return SmallerCloseSeq(window, 0.1f, 0.08f)
            .Join(SmallerCloseSeq(background, 0.1f, 0.08f));
    }

    public static Sequence AppOpenAnimation(GameObject window, GameObject background)
    {
        return BiggerOpenSeq(background, 0.1f, 0.08f)
            .AppendInterval(0.12f)
            .AppendCallback(() => window.SetActive(true));
    }

    public static Sequence ToastAnimation(GameObject window, float fadeTime, float delay)
    {
        return FadeInCanvasGroupSeq(window, fadeTime)
            .AppendInterval(delay)
            .Append(FadeOutCanvasGroupSeq(window, fadeTime));
    }

    /************************************************************
    * [팝업 애니메이션]
    * 
    * 팝업과 관련된 애니메이션
    ************************************************************/

    public static Sequence PopupOpenAnimation(GameObject window)
    {
        return FadeInCanvasGroupSeq(window, 0.1f);
    }

    public static Sequence PopupCloseAnimation(GameObject window)
    {
        return FadeOutCanvasGroupSeq(window, 0.1f);
    }

    /************************************************************
    * [애니메이션]
    * 
    * DOTween을 이용한 각 애니메이션 동작 시퀀스 관리
    ************************************************************/

    private static Sequence ToastWindowSeq(GameObject window, float t, Action openHandler = null)
    {
        Vector2 loc = window.transform.localPosition;
        window.transform.localPosition = new Vector2(loc.x, loc.y - window.GetComponent<RectTransform>().rect.height / 4);

        return DOTween.Sequence()
            .OnStart(() =>
            {
                window.SetActive(true);
                openHandler?.Invoke();
            })
            .Append(window.transform.DOLocalMoveY(loc.y, t).SetEase(Ease.OutQuad));
    }

    private static Sequence FadeInSeq(GameObject obj, float time)
    {
        Image image = obj.GetComponent<Image>();
        float origin = image.color.a;

        return DOTween.Sequence()
            .OnStart(() =>
            {
                Color color = image.color;
                color.a = 0;

                image.color = color;

                obj.SetActive(true);
            })
            .Append(image.DOFade(origin, time));
    }

    private static Sequence FadeInCanvasGroupSeq(GameObject obj, float time)
    {
        CanvasGroup group = obj.GetComponent<CanvasGroup>();
        float origin = group.alpha;

        return DOTween.Sequence()
            .OnStart(() =>
            {
                group.alpha = 0;

                obj.SetActive(true);
            })
            .Append(group.DOFade(origin, time));
    }

    private static Sequence FadeInAppSeq(GameObject appWindow, float time, Action openHandler = null)
    {
        CanvasGroup group = appWindow.GetComponent<CanvasGroup>();
        float origin = group.alpha;

        return DOTween.Sequence()
            .OnStart(() =>
            {
                group.alpha = 0;

                appWindow.SetActive(true);
                openHandler?.Invoke();
            })
            .Append(group.DOFade(origin, time));
    }

    private static Sequence FadeOutSeq(GameObject obj, float time)
    {
        if (obj.activeSelf == true)
        {
            Image image = obj.GetComponent<Image>();
            float origin = image.color.a;

            return DOTween.Sequence()
                .Append(image.DOFade(0.0f, time))
                .OnComplete(() =>
                {
                    obj.SetActive(false);

                    Color color = image.color;
                    color.a = origin;

                    image.color = color;
                });
        }

        return DOTween.Sequence();
    }

    private static Sequence FadeOutCanvasGroupSeq(GameObject obj, float time)
    {
        CanvasGroup group = obj.GetComponent<CanvasGroup>();
        float origin = group.alpha;

        return DOTween.Sequence()
            .Append(group.DOFade(0.0f, time))
            .OnComplete(() =>
            {
                obj.SetActive(false);

                group.alpha = origin;
            });
    }

    private static Sequence BiggerSeq(GameObject window, float startSize, float resultSize, float t)
    {
        return DOTween.Sequence()
            .OnStart(() =>
            {
                window.transform.localScale = new Vector3(startSize, startSize, 1);
            })
            .Append(window.transform.DOScale(new Vector3(resultSize, resultSize, 1), t));
    }

    private static Sequence BiggerOpenSeq(GameObject window, float startSize, float t)
    {
        window.SetActive(true);

        return BiggerSeq(window, startSize, 1f, t);
    }

    private static Sequence SmallerSeq(GameObject window, float startSize, float resultSize, float t)
    {
        return DOTween.Sequence()
            .OnStart(() =>
            {
                window.transform.localScale = new Vector3(startSize, startSize, 1);
            })
            .Append(window.transform.DOScale(new Vector3(resultSize, resultSize, 1), t).SetEase(Ease.OutCubic));
    }

    private static Sequence SmallerCloseSeq(GameObject window, float size, float t)
    {
        return SmallerSeq(window, 1f, size, t)
            .OnComplete(() =>
            {
                window.transform.localScale = new Vector3(1, 1, 1);
                window.SetActive(false);
            });
    }
}