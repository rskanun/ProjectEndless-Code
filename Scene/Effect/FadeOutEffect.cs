using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class FadeOutEffect : MonoBehaviour, ITransitionEffect
{
    [SerializeField] private Image panel;
    [Title("Settings")]
    [SerializeField] private float delay = 1.0f;

    public UniTask PlayEffect()
    {
        return panel.DOFade(0.0f, delay)
            .SetUpdate(true)
            .ToUniTask();
    }
}