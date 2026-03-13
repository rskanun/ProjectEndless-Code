using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class BlurFadeInEffect : MonoBehaviour, ITransitionEffect
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
                blurMaterial.SetFloat("_Radius", blurValue);
            })
            .Append(background.DOFade(0, delay))
            .Join(DOTween.To(() => blurMaterial.GetFloat("_Radius"), x => blurMaterial.SetFloat("_Radius", x), 0, delay))
            .SetUpdate(true)
            .ToUniTask();
    }
}