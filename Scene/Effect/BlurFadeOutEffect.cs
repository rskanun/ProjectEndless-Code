using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class BlurFadeOutEffect : MonoBehaviour, ITransitionEffect
{
    [Header("사용 오브젝트")]
    [SerializeField] private CanvasGroup background;
    [SerializeField] private Material blurMaterial;

    public UniTask PlayEffect()
    {
        float delay = 1f;
        float blurValue = 10.0f;

        return DOTween.Sequence()
            .OnStart(() =>
            {
                blurMaterial.SetFloat("_Radius", 0);
            })
            .Append(background.DOFade(1, delay))
            .Join(DOTween.To(() => blurMaterial.GetFloat("_Radius"), x => blurMaterial.SetFloat("_Radius", x), blurValue, delay))
            .OnComplete(() =>
            {
                blurMaterial.SetFloat("_Radius", 0);
            })
            .SetUpdate(true)
            .ToUniTask();
    }
}