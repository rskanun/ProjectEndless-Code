using System.Collections;
using UnityEngine;

public abstract class Effect : MonoBehaviour
{
    protected Coroutine effectCoroutine;

    public virtual void ActiveEffect(float time)
    {
        // 이미 진행 중인 이펙트가 있으면 종료
        if (effectCoroutine != null)
            StopCoroutine(effectCoroutine);

        // 코루틴 진행
        effectCoroutine = StartCoroutine(EffectCoroutine(time));
    }

    private IEnumerator EffectCoroutine(float time)
    {
        SetActive(true);
        yield return new WaitForSecondsRealtime(time);
        SetActive(false);

        effectCoroutine = null;
    }

    public abstract void SetActive(bool active);
}