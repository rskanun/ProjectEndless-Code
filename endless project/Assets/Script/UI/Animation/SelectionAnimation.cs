using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class SelectionAnimation
{
    public static void openSelectionAnimation(GameObject window, List<GameObject> options, float height)
    {
        float h = 104;
        float sec = 0.25f;

        openSelectionSeq(window, options, height, h, sec);
    }

    private static Sequence openSelectionSeq(GameObject window, List<GameObject> options, float height, float minH, float sec)
    {
        RectTransform rect = window.GetComponent<RectTransform>();

        return DOTween.Sequence()
            .OnStart(() =>
            {
                rect.sizeDelta = new Vector2(rect.rect.width, minH);
            })
            .Append(rect.DOSizeDelta(new Vector2(rect.rect.width, height), sec).SetEase(Ease.OutCubic))
            .OnComplete(() =>
            {
                foreach (GameObject option in options)
                {
                    option.SetActive(true);
                }
            });
    }
}