using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Assets.Script.Interface.Menu.App;

namespace Assets.Script.UI
{
    public class AppAnimation
    {
        public static void openAppAnimation(GameObject window, GameObject background, GameObject homeScreen)
        {
            biggerOpenSeq(background, 0.1f, 0.08f)
                    .AppendInterval(0.12f)
                    .Append(windowToastSeq(window, 0.2f));
        }

        public static void closeAppAnimation(GameObject window, GameObject background, GameObject homeScreen)
        {
            smallerCloseSeq(window, 0.1f, 0.08f)
                .Join(smallerCloseSeq(background, 0.1f, 0.08f));
        }

        public static void openMenuAnimation(GameObject window, float openRotate, float closeRotate)
        {
            openMenuSeq(window, openRotate, closeRotate);
        }

        public static void closeMenuAnimation(GameObject window, float openRotate, float closeRotate)
        {
            closeMenuSeq(window, openRotate, closeRotate);
        }

        public static void openSimpleAppAnimation(GameObject window, GameObject background, GameObject homeScreen)
        {
            biggerOpenSeq(background, 0.1f, 0.08f)
                    .OnComplete(() => window.SetActive(true));
        }

        public static void alertAnimation(GameObject alert, float fadeTime ,float delay)
        {
            fadeInSeq(alert, fadeTime)
                .AppendInterval(delay)
                .Append(fadeOutSeq(alert, fadeTime));
        }

        public static void showHomeScreenAnimation(GameObject homeScreen)
        {
            biggerSeq(homeScreen, 0.5f, 0.15f);
        }

        public static void hideHomeScreenAnimation(GameObject homeScreen)
        {
            smallerSeq(homeScreen, 0.5f, 0.15f);
        }

        /************************************************************
        * [애니메이션]
        * 
        * DOTween을 이용한 각 애니메이션 동작 시퀀스 관리
        ************************************************************/

        private static Sequence windowToastSeq(GameObject window, float t)
        {
            Vector2 loc = window.transform.localPosition;
            window.transform.localPosition = new Vector2(loc.x, loc.y - window.GetComponent<RectTransform>().rect.height / 4);

            return DOTween.Sequence()
                .OnStart(() => window.SetActive(true))
                .Append(window.transform.DOLocalMoveY(loc.y, t).SetEase(Ease.OutQuad));
        }

        private static Sequence openMenuSeq(GameObject window, float openRotate, float closeRotate)
        {
            return DOTween.Sequence()
                .OnStart(() =>
                {
                    window.transform.localRotation = Quaternion.Euler(0, 0, closeRotate);
                    window.SetActive(true);
                })
                .Append(window.transform.DORotate(new Vector3(0, 0, openRotate), 0.19f).SetEase(Ease.OutSine));
        }

        private static Sequence closeMenuSeq(GameObject window, float openRotate, float closeRotate)
        {
            return DOTween.Sequence()
                .Append(window.transform.DORotate(new Vector3(0, 0, closeRotate), 0.19f).SetEase(Ease.InQuad))
                .OnComplete(() => {
                    window.SetActive(false);
                    window.transform.localRotation = Quaternion.Euler(0, 0, openRotate);
                    });
        }

        private static Sequence fadeInSeq(GameObject obj, float time)
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

        private static Sequence fadeOutSeq(GameObject obj, float time)
        {
            if(obj.activeSelf == true)
            {
                Image image = obj.GetComponent<Image>();

                return DOTween.Sequence()
                    .Append(image.DOFade(0.0f, time))
                    .OnComplete(() =>
                    {
                        obj.SetActive(false);

                        Color color = image.color;
                        color.a = 1;

                        image.color = color;
                    });
            }

            return DOTween.Sequence();
        }

        private static Sequence biggerSeq(GameObject window, float startSize, float t)
        {
            return DOTween.Sequence()
                .OnStart(() =>
                {
                    window.transform.localScale = new Vector3(startSize, startSize);
                })
                .Append(window.transform.DOScale(new Vector3(1, 1), t));
        }

        private static Sequence biggerOpenSeq(GameObject window, float startSize, float t)
        {
            window.SetActive(true);
            return biggerSeq(window, startSize, t);

        }

        private static Sequence smallerSeq(GameObject window, float size, float t)
        {
            return DOTween.Sequence()
                .Append(window.transform.DOScale(new Vector3(size, size), t))
                .OnComplete(() =>
                {
                    window.transform.localScale = new Vector3(1, 1);
                });
        }

        private static Sequence smallerCloseSeq(GameObject window, float size, float t)
        {
            return smallerSeq(window, size, t)
                .OnComplete(() =>
                {
                    window.transform.localScale = new Vector3(1, 1);
                    window.SetActive(false);
                });
        }
    }
}