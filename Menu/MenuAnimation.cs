using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class MenuAnimation
{
    // 메뉴 열리고 닫히는 각도
    private const float closeRotate = 70, openRotate = 0;

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

    public static Sequence MenuOpenAnimation(GameObject phone, GameObject screenPanel, GameObject appGroup, GameObject face)
    {
        return OpenMenuSeq(phone, screenPanel, appGroup, face);
    }

    public static Sequence MenuCloseAnimation(GameObject phone, GameObject screenPanel, GameObject face)
    {
        return CloseMenuSeq(phone, screenPanel, face);
    }

    public static Sequence AppOpenAnimation(GameObject window, GameObject background)
    {
        return BiggerOpenSeq(background, 0.1f, 0.08f)
                .AppendInterval(0.12f)
                .Append(FadeInSeq(window, 0.2f));
    }

    public static Sequence ToastAnimation(GameObject window, float fadeTime, float delay)
    {
        return FadeInCanvasGroupSeq(window, fadeTime)
            .AppendInterval(delay)
            .Append(FadeOutCanvasGroupSeq(window, fadeTime));
    }

    public static Sequence HomeScreenShowAnimation(GameObject homeScreen)
    {
        return BiggerSeq(homeScreen, 0.5f, 1f, 0.15f);
    }

    public static Sequence HomeScreenHideAnimation(GameObject homeScreen)
    {
        return SmallerSeq(homeScreen, 1f, 0.5f, 0.15f);
    }

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

    private static Sequence ToastWindowSeq(GameObject window, float t)
    {
        Vector2 loc = window.transform.localPosition;
        window.transform.localPosition = new Vector2(loc.x, loc.y - window.GetComponent<RectTransform>().rect.height / 4);

        return DOTween.Sequence()
            .OnStart(() => window.SetActive(true))
            .Append(window.transform.DOLocalMoveY(loc.y, t).SetEase(Ease.OutQuad));
    }

    private static Sequence OpenMenuSeq(GameObject phone, GameObject screenPanel, GameObject appGroup, GameObject face)
    {
        float delay = 0.16f;
        float menuOpenDelay = 0.19f;
        float screenOpenDelay = 0.05f;
        float loadAppDelay = 0.1f;
        float startScale = 10f, resultScale = 1f;

        Vector2 oriPos = face.transform.position;
        Vector3 oriRotate = face.transform.rotation.eulerAngles;

        Image darkPanel = screenPanel.GetComponent<Image>();

        // 휴대폰을 꺼내드는 모션
        Sequence retrievePhoneSeq = DOTween.Sequence()
            .OnStart(() =>
            {
                face.transform.localRotation = Quaternion.Euler(0, 0, -closeRotate);
                phone.transform.localRotation = Quaternion.Euler(0, 0, closeRotate);
                phone.SetActive(true);

            })
            .Append(phone.transform.DORotate(new Vector3(0, 0, openRotate), menuOpenDelay).SetEase(Ease.OutSine))
            .Join(RotateFace(face, oriPos, oriRotate, menuOpenDelay));

        // 화면을 키는 모션
        Sequence turnOnScreenSeq = DOTween.Sequence()
            .OnStart(() =>
            {
                appGroup.transform.localScale = new Vector3(startScale, startScale, 1);
                appGroup.SetActive(true);
                screenPanel.SetActive(true);
            })
            .Append(darkPanel.DOFade(0f, screenOpenDelay))
            .Append(appGroup.transform.DOScale(new Vector3(resultScale, resultScale, 1), loadAppDelay).SetEase(Ease.OutSine));

        return DOTween.Sequence()
            .Append(retrievePhoneSeq)
            .AppendInterval(delay)
            .Append(turnOnScreenSeq);
    }

    private static Sequence CloseMenuSeq(GameObject phone, GameObject screenPanel, GameObject face)
    {
        float delay = 0.16f;
        float screenCloseDelay = 0.1f;
        float menuCloseDelay = 0.19f;

        Vector2 oriPos = face.transform.position;
        Vector3 oriRotate = face.transform.rotation.eulerAngles;

        Image darkPanel = screenPanel.GetComponent<Image>();

        // 화면을 끄는 모션
        Sequence turnOffScreenSeq = DOTween.Sequence()
            .Append(darkPanel.DOFade(1f, screenCloseDelay));

        // 휴대폰을 집어넣는 모션
        Sequence insertPhoneSeq = DOTween.Sequence()
            .Append(phone.transform.DORotate(new Vector3(0, 0, closeRotate), menuCloseDelay).SetEase(Ease.InQuad))
            .Join(RotateFace(face, oriPos, oriRotate, menuCloseDelay))
            .OnComplete(() =>
            {
                phone.SetActive(false);
                phone.transform.localRotation = Quaternion.Euler(0, 0, openRotate);
                face.transform.position = oriPos;
                face.transform.rotation = Quaternion.Euler(oriRotate);
            });

        return DOTween.Sequence()
            .Append(turnOffScreenSeq)
            .AppendInterval(delay)
            .Append(insertPhoneSeq);
    }

    private static Tweener RotateFace(GameObject face, Vector2 oriPos, Vector3 oriRotate, float delay)
    {
        return DOTween.To(() => 0, x =>
        {
            face.transform.position = oriPos;
            face.transform.rotation = Quaternion.Euler(oriRotate);
        }, 0, delay);
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