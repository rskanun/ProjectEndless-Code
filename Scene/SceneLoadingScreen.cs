using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadingScreen : MonoBehaviour
{
    [Header("씬 전환 연출")]
    [SerializeField] private BlurFadeOutEffect blurFadeOut;
    [SerializeField] private BlurFadeInEffect blurFadeIn;

    [Header("씬 로딩 애니메이션")]
    [SerializeField] private LoadingAnimation loading;
    [SerializeField] private ClockLoadingAnimation clockLoading;

    private Dictionary<SceneFadeEffect, Action<Action>> sceneEffects;
    private Dictionary<LoadingScreen, Action<List<string>, List<string>, UnloadSceneOptions, Action>> loadingAnimations;
    private Coroutine loadingCoroutine;
    private bool isPlayAnimation;

    private void Awake()
    {
        sceneEffects = new Dictionary<SceneFadeEffect, Action<Action>>
        {
            { SceneFadeEffect.BlurFadeOut, blurFadeOut.OnPlayEffect },
            { SceneFadeEffect.BlurFadeIn, blurFadeIn.OnPlayEffect }
        };

        loadingAnimations = new Dictionary<LoadingScreen, Action<List<string>, List<string>, UnloadSceneOptions, Action>>
        {
            { LoadingScreen.Loading, loading.OnPlayAnimation },
            { LoadingScreen.ClockLoading, clockLoading.OnPlayAnimation }
        };
    }

    private void OnEnable()
    {
        LoadSceneManager.Instance.RegisterManager(this);
    }

    private void OnDisable()
    {
        LoadSceneManager.Instance.RemoveManager();
    }

    /************************************************************
     * [씬 불러오기]
     * 
     * 현재 활성화 된 씬 중에 필요없는 씬을 지우고 필요한 씬 불러오기
     ************************************************************/

    public void EnableScreen(List<string> loadScenes, List<string> unloadScenes, SceneFadeEffect startEffect, SceneFadeEffect endEffect, LoadingScreen screen)
    {
        if (loadingCoroutine != null)
        {
            // 이미 로딩 중인 경우 무시
            return;
        }

        loadingCoroutine = StartCoroutine(SceneLoading(loadScenes, unloadScenes, startEffect, endEffect, screen));
    }

    private IEnumerator SceneLoading(List<string> loadScenes, List<string> unloadScenes, SceneFadeEffect startEffect, SceneFadeEffect endEffect, LoadingScreen screen)
    {
        // 로딩 중엔 어떠한 키 입력도 받지 않기
        ControlContext.Instance.KeyLock();

        // 로딩 화면을 띄우기 위한 전환 연출 실행
        PlayTransitionEffect(startEffect);
        yield return new WaitWhile(() => isPlayAnimation);

        // 

        // 로딩이 끝났다면 키 입력 받기
        ControlContext.Instance.KeyUnlock();

        loadingCoroutine = null;
    }

    /************************************************************
     * [씬 전환 연출]
     * 
     * 로딩 씬 전환 간에 띄울 연출 관리
     ************************************************************/

    private void PlayTransitionEffect(SceneFadeEffect type)
    {
        isPlayAnimation = true;
        sceneEffects[type]?.Invoke(() => isPlayAnimation = false);
    }

    /************************************************************
     * [로딩 애니메이션]
     * 
     * 씬을 불러올 동안의 로딩 간에 띄울 애니메이션 관리
     ************************************************************/

    private void EnableLoadingScreen(List<string> loadScenes, List<string> unloadScenes, LoadingScreen screen)
    {
        isPlayAnimation = true;
        loadingAnimations[screen]?.Invoke(loadScenes, unloadScenes, () => isPlayAnimation = false);
    }
}