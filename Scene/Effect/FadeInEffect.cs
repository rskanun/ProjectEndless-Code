using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class FadeInEffect : MonoBehaviour, ITransitionEffect
{
    [SerializeField] private Image panel;

    public void OnPlayEffect(Action completeAction)
    {
        float delay = 1.0f;

        DOTween.Sequence()
            .Append(panel.DOFade(1.0f, delay))
            .AppendCallback(() => completeAction?.Invoke());
    }
}