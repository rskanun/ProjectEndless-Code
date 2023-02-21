using UnityEngine;
using DG.Tweening;

namespace Assets.Script.UI
{
    public class AppAnimation : MonoBehaviour
    {
        public GameObject maskingImage;

        public void openAppAnimation(GameObject window)
        {
            appOpenSeq()
                .Append(windowOpenSeq(window));
        }

        public void closeAppAnimation(GameObject window)
        {
            appCloseSeq(window);
        }

        public void openMenuAnimation(GameObject window, float openRotate, float closeRotate)
        {
            openMenuSeq(window, openRotate, closeRotate);
        }

        public void closeMenuAnimation(GameObject window, float openRotate, float closeRotate)
        {
            closeMenuSeq(window, openRotate, closeRotate);
        }

        /************************************************************
        * [애니메이션]
        * 
        * DOTween을 이용한 각 애니메이션 동작 시퀀스 관리
        ************************************************************/

        public Sequence appOpenSeq()
        {
            return DOTween.Sequence()
                .OnStart(() => {
                    maskingImage.transform.localScale = new Vector3(0.1f, 0.1f);
                    maskingImage.SetActive(true);
                    })
                .Append(maskingImage.transform.DOScale(new Vector3(1, 1), 0.08f))
                .AppendInterval(0.12f);
        }

        public Sequence windowOpenSeq(GameObject window)
        {
            Vector2 loc = window.transform.localPosition;
            window.transform.localPosition = new Vector2(loc.x, loc.y - window.GetComponent<RectTransform>().rect.height / 4);

            return DOTween.Sequence()
                .OnStart(() => window.SetActive(true))
                .Append(window.transform.DOLocalMoveY(loc.y, 0.2f).SetEase(Ease.OutQuad));
        }

        public Sequence appCloseSeq(GameObject window)
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

        public Sequence openMenuSeq(GameObject window, float openRotate, float closeRotate)
        {
            return DOTween.Sequence()
                .OnStart(() => {
                    window.transform.localRotation = Quaternion.Euler(0, 0, closeRotate);
                    window.SetActive(true);
                    })
                .Append(window.transform.DORotate(new Vector3(0, 0, openRotate), 0.19f).SetEase(Ease.OutSine));
        }

        public Sequence closeMenuSeq(GameObject window, float openRotate, float closeRotate)
        {
            return DOTween.Sequence()
                .Append(window.transform.DORotate(new Vector3(0, 0, closeRotate), 0.19f).SetEase(Ease.InQuad))
                .OnComplete(() => {
                    window.SetActive(false);
                    window.transform.localRotation = Quaternion.Euler(0, 0, openRotate);
                    });
        }
    }
}