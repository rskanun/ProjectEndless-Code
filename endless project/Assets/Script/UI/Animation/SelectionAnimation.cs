using System.Collections;
using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

namespace Assets.Script.UI.Animation
{
    public class SelectionAnimation : MonoBehaviour
    {
        public void openSelectionAnimation(GameObject window, List<GameObject> options, float height)
        {
            float h = 104;
            float sec = 0.25f;

            openSelectionSeq(window, options, height, h, sec);
        }

        private Sequence openSelectionSeq(GameObject window, List<GameObject> options, float height, float minH, float sec)
        {
            RectTransform rect = window.GetComponent<RectTransform>();

            return DOTween.Sequence()
                .OnStart(() =>
                {
                    rect.sizeDelta = new Vector2(rect.rect.width, minH);
                    window.SetActive(true);
                })
                .Append(rect.DOSizeDelta(new Vector2(rect.rect.width, height), sec).SetEase(Ease.OutCubic))
                .OnComplete(() =>
                {
                    foreach(GameObject option in options)
                    {
                        option.SetActive(true);
                    }
                });
        }
    }
}