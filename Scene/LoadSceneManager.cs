using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Threading.Tasks;

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

public class LoadSceneManager
{

    private static LoadSceneManager _instance;
    public static LoadSceneManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new LoadSceneManager();

            return _instance;
        }
    }

    public static SceneLoadingScreen.LoadingCallBack loadingCallBack;

    private SceneLoadingScreen loadingScreen;
    private SceneResource resource;

    public LoadSceneManager()
    {
        resource = SceneResource.Instance;
    }

    public void RegisterManager(SceneLoadingScreen loadingScreen)
    {
        this.loadingScreen = loadingScreen;
    }

    public void RemoveManager()
    {
        loadingScreen = null;
    }

    public void InitCallBack()
    {
        // 콜백 함수 초기화
        loadingCallBack = () => { };
    }

    /************************************************************
     * [씬 전환]
     * 
     * 상황에 따른 씬 전환 시 띄울 애니메이션과 활성화 할 씬 관리
     ************************************************************/

    public void LoadTitleScene(SceneFadeEffect startEffect, SceneFadeEffect endEffect, LoadingScreen screen)
    {
        LoadScene(resource.TitleRequireScenes, null, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects, startEffect, endEffect, screen);
    }

    public void LoadFieldScene(string loadMap, UnloadSceneOptions unloadOptions, SceneFadeEffect startEffect, SceneFadeEffect endEffect, LoadingScreen screen)
    {
        LoadScene(resource.FieldRequireScenes, loadMap, unloadOptions, startEffect, endEffect, screen);
    }

    public void LoadBattleScene(string loadMap, UnloadSceneOptions unloadOptions, SceneFadeEffect startEffect, SceneFadeEffect endEffect, LoadingScreen screen)
    {
        LoadScene(resource.BattleRequireScenes, loadMap, unloadOptions, startEffect, endEffect, screen);
    }

    private async void LoadScene(List<string> requireScenes, string loadMap, UnloadSceneOptions unloadOptions, SceneFadeEffect startEffect, SceneFadeEffect endEffect, LoadingScreen screen)
    {
        List<string> activeScenes = FindActiveScenes();

        // 활성화된 씬과 필요한 씬 비교 후 각각 로드할 씬, 언로드할 씬 리스트 생성
        List<string> unloadScenes = activeScenes.Except(requireScenes).ToList();
        List<string> loadScenes = requireScenes.Except(activeScenes).ToList();

        // 같은 맵이어도 다시 로드하기 위해 활성화 할 목록에 추가
        if (!string.IsNullOrEmpty(loadMap)) loadScenes.Add(loadMap);

        // 로딩씬 불러오기
        await LoadSceneAsyncTask(resource.LoadingScene, LoadSceneMode.Additive);

        // 로딩 간에 실행될 함수 설정
        loadingCallBack += () => InitCallBack();
        loadingScreen.loadingCallBack = loadingCallBack;

        // 로딩화면 띄우기
        loadingScreen.EnableScreen(loadScenes, unloadScenes, unloadOptions, startEffect, endEffect, screen);
    }

    private async Task LoadSceneAsyncTask(string sceneName, LoadSceneMode mode)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, mode);
        if (asyncLoad == null) return;

        // TaskCompletionSource를 사용하여 완료될 때까지 대기
        TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
        asyncLoad.completed += _ => tcs.SetResult(true);
        await tcs.Task;
    }

    private List<string> FindActiveScenes()
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