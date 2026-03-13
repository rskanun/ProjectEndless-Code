using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class SceneLoadingScreen : MonoBehaviour
{
    [Header("씬 전환 연출")]
    [SerializeField] private BlurFadeOutEffect blurFadeOut;
    [SerializeField] private BlurFadeInEffect blurFadeIn;

    [Header("씬 로딩 애니메이션")]
    [SerializeField] private LoadingAnimation loading;
    [SerializeField] private ClockLoadingAnimation clockLoading;

    private ILoadAnimation curAnim;
    private Dictionary<SceneFadeEffect, ITransitionEffect> sceneEffects;
    private Dictionary<LoadingScreen, ILoadAnimation> loadingAnimations;

    private void Awake()
    {
        sceneEffects = new Dictionary<SceneFadeEffect, ITransitionEffect>
        {
            { SceneFadeEffect.BlurFadeOut, blurFadeOut },
            { SceneFadeEffect.BlurFadeIn, blurFadeIn }
        };

        loadingAnimations = new Dictionary<LoadingScreen, ILoadAnimation>
        {
            { LoadingScreen.Loading, loading },
            { LoadingScreen.ClockLoading, clockLoading }
        };
    }

    /************************************************************
     * [씬 전환 연출]
     * 
     * 로딩 씬 전환 간에 띄울 연출 관리
     ************************************************************/

    public UniTask PlayTransitionEffect(SceneFadeEffect type)
    {
        // 등록된 연출 UniTask로 비동기 실행
        if (sceneEffects.TryGetValue(type, out var effect) && effect != null)
        {
            return effect.PlayEffect();
        }

        // 등록된 연출이 없다면 바로 완료
        return UniTask.CompletedTask;
    }

    /************************************************************
     * [로딩 애니메이션]
     * 
     * 씬을 불러올 동안의 로딩 간에 띄울 애니메이션 관리
     ************************************************************/

    public UniTask ShowLoadingScreen(LoadingScreen screen)
    {
        // 진행 중인 애니메이션 강제 종료
        if (curAnim != null)
        {
            Debug.LogWarning($"이미 로딩 애니메이션이 실행 중입니다: {nameof(curAnim)}");
            return UniTask.CompletedTask;
        }

        // 등록된 애니메이션 UniTask로 비동기 실행
        if (loadingAnimations.TryGetValue(screen, out var loadAnim) && loadAnim != null)
        {
            curAnim = loadAnim;
            return loadAnim.PlayAnimation();
        }

        // 등록된 애니메이션이 없다면 바로 완료
        Debug.LogWarning($"{screen} 타입의 애니메이션이 등록되어 있지 않습니다.");
        return UniTask.CompletedTask;
    }

    public void HideLoadingScreen()
    {
        // 진행중인 애니메이션이 없다면 무시
        if (curAnim == null) return;

        // 애니메이션 종료
        curAnim.StopAnimation();
        curAnim = null;
    }
}