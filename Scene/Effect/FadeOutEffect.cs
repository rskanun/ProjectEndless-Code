using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class FadeOutEffect : MonoBehaviour, ITransitionEffect
{
    [SerializeField] private Image panel;

    public void OnPlayEffect(Action completeAction)
    {
        float delay = 1.0f;

        DOTween.Sequence()
            .Append(panel.DOFade(0.0f, delay))
            .AppendCallback(() => completeAction?.Invoke());
    }
}