using System;
using DG.Tweening;
using UnityEngine;

public class BlurFadeInEffect : MonoBehaviour, ITransitionEffect
{
    [Header("사용 오브젝트")]
    [SerializeField] private CanvasGroup background;
    [SerializeField] private Material blurMaterial;

    public void OnPlayEffect(Action completeAction)
    {
        float delay = 1f;
        float blurValue = 10.0f;

        DOTween.Sequence()
            .OnStart(() =>
            {
                blurMaterial.SetFloat("_Radius", blurValue);
            })
            .Append(background.DOFade(0, delay))
            .Join(DOTween.To(() => blurMaterial.GetFloat("_Radius"), x => blurMaterial.SetFloat("_Radius", x), 0, delay))
            .OnComplete(() =>
            {
                completeAction?.Invoke();
            });
    }
}