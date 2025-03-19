using System;
using DG.Tweening;
using UnityEngine;

public class SceneAnimationManager : MonoBehaviour
{
    [Header("연출 오브젝트")]
    [SerializeField] private CanvasGroup background;
    [SerializeField] private Material blurMaterial;

    private void OnEnable()
    {
        LoadSceneManager.Instance.RegisterManager(this);
    }

    private void OnDisable()
    {
        LoadSceneManager.Instance.RemoveManager();
    }

    public void PlayAnimation(LoadAnimationType effectType, Action completeAction = null)
    {

    }

    private void BlurFadeOutAnimation(Action completeAction = null)
    {
        float delay = 1f;

        DOTween.Sequence()
            .OnStart(() =>
            {
                background.blocksRaycasts = true;

                ControlContext.Instance.KeyLock();
            })
            .Append(background.DOFade(1, delay))
            .Join(DOTween.To(() => blurMaterial.GetFloat("_Radius"), x => blurMaterial.SetFloat("_Radius", x), 10f, delay))
            .OnComplete(() =>
            {
                blurMaterial.SetFloat("_Radius", 0);

                // SceneManager.LoadScene(sceneName);
                completeAction?.Invoke();
            });
    }

    private void BlurFadeInAnimation(Action completeAction = null)
    {
        float delay = 1f;

        DOTween.Sequence()
            .OnStart(() =>
            {
                blurMaterial.SetFloat("_Radius", 10f);
            })
            .Append(background.DOFade(0, delay))
            .Join(DOTween.To(() => blurMaterial.GetFloat("_Radius"), x => blurMaterial.SetFloat("_Radius", x), 0, delay))
            .OnComplete(() =>
            {
                background.blocksRaycasts = false;

                ControlContext.Instance.KeyUnlock();
            });
    }
}