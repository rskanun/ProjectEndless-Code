using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Threading.Tasks;
using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;

public enum SceneFadeEffect
{
    BlurFadeIn,
    BlurFadeOut,
}

public enum LoadingScreen
{
    Loading,
    ClockLoading
}

public static class SceneLoadManager
{
    public static Action onLoaded;

    /************************************************************
     * [씬 전환]
     * 
     * 상황에 따른 씬 전환 시 띄울 애니메이션과 활성화 할 씬 관리
     ************************************************************/

    public static void LoadTitleScene(SceneFadeEffect startEffect, SceneFadeEffect endEffect, LoadingScreen screen)
    {
        // 로딩 이후 현재 게임 상태를 타이틀로 변경하기
        onLoaded += () => GameData.Instance.State = GameState.Title;

        // 메인 씬 지정
        onLoaded += () => SceneManager.SetActiveScene(SceneManager.GetSceneByName(SceneResource.Instance.TitleMainScene));

        // 씬 변경
        LoadScene(SceneResource.Instance.TitleRequireScenes, null, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects, startEffect, endEffect, screen);
    }

    public static void LoadFieldScene(string loadMap, UnloadSceneOptions unloadOptions, SceneFadeEffect startEffect, SceneFadeEffect endEffect, LoadingScreen screen)
    {
        // 로딩 이후 현재 게임 상태를 필드로 변경하기
        onLoaded += () => GameData.Instance.State = GameState.Field;

        // 메인 씬 지정
        onLoaded += () => SceneManager.SetActiveScene(SceneManager.GetSceneByName(SceneResource.Instance.FieldMainScene));

        // 씬 변경
        LoadScene(SceneResource.Instance.FieldRequireScenes, loadMap, unloadOptions, startEffect, endEffect, screen);
    }

    public static void LoadBattleScene(string loadMap, UnloadSceneOptions unloadOptions, SceneFadeEffect startEffect, SceneFadeEffect endEffect, LoadingScreen screen)
    {
        // 로딩 이후 현재 게임 상태를 전투로 변경하기
        onLoaded += () => GameData.Instance.State = GameState.Battle;

        // 메인 씬 지정
        onLoaded += () => SceneManager.SetActiveScene(SceneManager.GetSceneByName(SceneResource.Instance.BattleMainScene));

        // 씬 변경
        LoadScene(SceneResource.Instance.BattleRequireScenes, loadMap, unloadOptions, startEffect, endEffect, screen);
    }

    private static async void LoadScene(List<string> requireScenes, string loadMap, UnloadSceneOptions unloadOptions, SceneFadeEffect startEffect, SceneFadeEffect endEffect, LoadingScreen screen)
    {
        List<string> activeScenes = FindActiveScenes();

        // 활성화된 씬과 필요한 씬 비교 후 각각 로드할 씬, 언로드할 씬 리스트 생성
        List<string> unloadScenes = activeScenes.Except(requireScenes).ToList();
        List<string> loadScenes = requireScenes.Except(activeScenes).ToList();

        // 같은 맵이어도 다시 로드하기 위해 활성화 할 목록에 추가
        if (!string.IsNullOrEmpty(loadMap)) loadScenes.Add(loadMap);

        // 씬 로드
        await SceneLoadingAsync(loadScenes, unloadScenes, unloadOptions, startEffect, endEffect, screen);
    }

    private static async UniTask SceneLoadingAsync(List<string> loadScenes, List<string> unloadScenes, UnloadSceneOptions unloadOptions, SceneFadeEffect startEffect, SceneFadeEffect endEffect, LoadingScreen screen)
    {
        try
        {
            // 로딩 간 키 입력 방지 및 시간 멈추기
            ControlContext.Instance.KeyLock();
            Time.timeScale = 0.0f;

            // 씬 변경 전 DOTween 애니메이션 파괴
            DOTween.Clear(true);

            // 로딩씬 불러오기
            var loadingScreen = await LoadLoadingSceneComponent(LoadSceneMode.Additive);
            if (loadingScreen == null) return;

            // 로딩 화면 전환 연출 실행
            await loadingScreen.PlayTransitionEffect(startEffect);

            // 씬 로딩
            await LoadingAsync(loadScenes, unloadScenes, unloadOptions, loadingScreen, screen);

            // 로딩 이후 콜백 실행
            onLoaded?.Invoke();
            onLoaded = null;

            // 멈춘 시간 되돌리기
            Time.timeScale = 1.0f;

            // 로딩 화면에서 전환 연출 실행
            await loadingScreen.PlayTransitionEffect(endEffect);
        }
        finally
        {
            // 키 입력 및 timeScale 되돌리기
            ControlContext.Instance.KeyUnlock();
            Time.timeScale = 1.0f;

            // 기다림 없이 로딩 씬 파괴
            SceneManager.UnloadSceneAsync(SceneResource.Instance.LoadingScene)
                .ToUniTask().Forget();
        }
    }

    private static async UniTask<SceneLoadingScreen> LoadLoadingSceneComponent(LoadSceneMode mode)
    {
        var loadingSceneName = SceneResource.Instance.LoadingScene;
        var asyncLoad = SceneManager.LoadSceneAsync(loadingSceneName, mode);

        if (asyncLoad == null) return null;

        // 씬 로드까지 대기
        await asyncLoad.ToUniTask();

        // 해당 씬에서 컴포넌트 찾기
        GameObject[] rootObjects = SceneManager.GetSceneByName(loadingSceneName).GetRootGameObjects();
        foreach (var go in rootObjects)
        {
            var component = go.GetComponentInChildren<SceneLoadingScreen>(true);
            if (component != null)
            {
                // 컴포넌트를 찾으면 즉시 반환하고 메서드를 종료
                return component;
            }
        }

        return null;
    }

    private static async UniTask LoadingAsync(List<string> loadScenes, List<string> unloadScenes, UnloadSceneOptions unloadOptions, SceneLoadingScreen loadingScreen, LoadingScreen screen)
    {
        var animTask = loadingScreen.ShowLoadingScreen(screen);
        var loadTask = SwapScenesAsync(loadScenes, unloadScenes, unloadOptions);

        // 애니메이션과 씬 로드 동시 진행
        await UniTask.WhenAll(animTask, loadTask);

        // 로딩 이후 애니메이션 종료
        loadingScreen.HideLoadingScreen();
    }

    private static async UniTask SwapScenesAsync(List<string> loadScenes, List<string> unloadScenes, UnloadSceneOptions unloadOptions)
    {
        // 언로드 병렬 작업
        var unloadTasks = new List<UniTask>(unloadScenes.Count);
        foreach (var scene in unloadScenes)
        {
            unloadTasks.Add(SceneManager.UnloadSceneAsync(scene, unloadOptions).ToUniTask());
        }

        // 언로드 작업 동시 진행
        await UniTask.WhenAll(unloadTasks);

        // 로드 병렬 작업
        var loadTasks = new List<UniTask>(loadScenes.Count);
        foreach (var scene in loadScenes)
        {
            loadTasks.Add(SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive).ToUniTask());
        }

        // 로드 작업 진행
        await UniTask.WhenAll(loadTasks);
    }

    private static List<string> FindActiveScenes()
    {
        // 현재 활성화 된 씬 이름을 리스트로 모아서 출력
        List<string> result = new List<string>();
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene activeScene = SceneManager.GetSceneAt(i);
            result.Add(activeScene.name);
        }

        return result;
    }
}