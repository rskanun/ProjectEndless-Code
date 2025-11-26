using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Threading.Tasks;
using System;
using Cysharp.Threading.Tasks;

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

    private static void InitCallBack()
    {
        // 콜백 함수 초기화
        onLoaded = () => { };
    }

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

        // 로딩씬 불러오기
        var loadingScreen = await LoadSceneAsyncTask(SceneResource.Instance.LoadingScene, LoadSceneMode.Additive);

        // 로딩씬 내부 loadingScreen을 불러오지 못했다면 종료
        if (loadingScreen == null)
        {
            Debug.LogWarning("로딩씬 내부에 SceneLoadingScreen 객체가 존재하지 않습니다!");
            InitCallBack();

            return;
        }

        // 로딩 이후 콜백함수 초기화
        onLoaded += () => InitCallBack();

        // 로딩화면 띄우기
        loadingScreen.EnableScreen(loadScenes, unloadScenes, unloadOptions, startEffect, endEffect, screen);
    }

    private static async UniTask<SceneLoadingScreen> LoadSceneAsyncTask(string sceneName, LoadSceneMode mode)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, mode);
        if (asyncLoad == null) return null;

        // 씬 로드까지 대기
        await asyncLoad.ToUniTask();

        // 해당 씬에서 컴포넌트 찾기
        GameObject[] rootObjects = SceneManager.GetSceneByName(sceneName).GetRootGameObjects();
        foreach (var go in rootObjects)
        {
            SceneLoadingScreen component = go.GetComponentInChildren<SceneLoadingScreen>(true);
            if (component != null)
            {
                // 컴포넌트를 찾으면 즉시 반환하고 메서드를 종료합니다.
                return component;
            }
        }

        return null;
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