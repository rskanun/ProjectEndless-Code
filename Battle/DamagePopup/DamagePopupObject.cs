using DG.Tweening;
using UnityEngine;

public class DamagePopupObject : MonoBehaviour
{
    private void OnEnable()
    {
        DOTween.Sequence()
        .Append(transform.DOMoveY(1.0f, 1.0f))
        .AppendInterval(1.0f)
        .OnComplete(() => Destroy(gameObject));
    }
}