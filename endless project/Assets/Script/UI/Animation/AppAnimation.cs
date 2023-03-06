using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace Assets.Script.UI
{
    public class AppAnimation
    {
        public static void openAppAnimation(GameObject window, GameObject background)
        {
            appOpenSeq(background)
                .Append(windowOpenSeq(window));
        }

        public static void closeAppAnimation(GameObject window, GameObject background)
        {
            appCloseSeq(window, background);
        }

        public static void openMenuAnimation(GameObject window, float openRotate, float closeRotate)
        {
            openMenuSeq(window, openRotate, closeRotate);
        }

        public static void closeMenuAnimation(GameObject window, float openRotate, float closeRotate)
        {
            closeMenuSeq(window, openRotate, closeRotate);
        }

        public static void openSimpleAppAnimation(GameObject window, GameObject background)
        {
            appOpenSeq(background)
                .OnComplete(() => window.SetActive(true));
        }

        public static void alertOnAnimation(GameObject alert)
        {
            float time = 0.15f;
            fadeInSeq(alert, time);
        }

        public static void alertOffAnimation(GameObject alert)
        {
            float time = 0.15f;
            fadeOutSeq(alert, time);
        }

        /************************************************************
        * [애니메이션]
        * 
        * DOTween을 이용한 각 애니메이션 동작 시퀀스 관리
        ************************************************************/

        private static Sequence appOpenSeq(GameObject maskingImage)
        {
            return DOTween.Sequence()
                .OnStart(() => {
                    maskingImage.transform.localScale = new Vector3(0.1f, 0.1f);
                    maskingImage.SetActive(true);
                    })
                .Append(maskingImage.transform.DOScale(new Vector3(1, 1), 0.08f))
                .AppendInterval(0.12f);
        }

        private static Sequence windowOpenSeq(GameObject window)
        {
            Vector2 loc = window.transform.localPosition;
            window.transform.localPosition = new Vector2(loc.x, loc.y - window.GetComponent<RectTransform>().rect.height / 4);

            return DOTween.Sequence()
                .OnStart(() => window.SetActive(true))
                .Append(window.transform.DOLocalMoveY(loc.y, 0.2f).SetEase(Ease.OutQuad));
        }

        private static Sequence appCloseSeq(GameObject window, GameObject maskingImage)
        {
            return DOTween.Sequence()
                .Append(maskingImage.transform.DOScale(new Vector3(0.7f, 0.7f), 0.015f))
                .Append(DOTween.Sequence()
                    .OnStart(() => window.SetActive(false))
                    .Append(maskingImage.transform.DOScale(new Vector3(0.1f, 0.1f), 0.065f)))
                .OnComplete(() =>
                {
                    maskingImage.transform.localScale = new Vector3(1, 1);
                    maskingImage.SetActive(false);
                });
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
    }
}