using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Collections;

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

public class SceneLoadManager : MonoBehaviour
{
    private static SceneLoadManager _instance;
    public static SceneLoadManager Instance
    {
        get
        {
            if (_instance != null) return _instance;

            // 씬 내에서 찾기
            _instance = FindObjectOfType<SceneLoadManager>();

            if (_instance == null)
            {
                // 해당 스크립트를 가진 오브젝트가 없다면 만들기
                GameObject obj = new GameObject("[SceneLoadManager]");
                _instance = obj.AddComponent<SceneLoadManager>();
            }

            return _instance;
        }
    }

    public static SceneLoadingScreen.LoadingCallBack loadingCallBack;

    private SceneLoadingScreen loadingScreen;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;

            // 씬 전환 시에도 유지
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            // 현재 instance에 등록된 게 해당 스크립트가 아니라면 파괴
            Destroy(gameObject);
        }
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
        // 로딩 간에 현재 게임 상태를 타이틀로 변경하기
        loadingCallBack += () => GameData.Instance.State = GameState.Title;

        // 메인 씬 지정
        loadingCallBack += () => SceneManager.SetActiveScene(SceneManager.GetSceneByName(SceneResource.Instance.TitleMainScene));

        // 씬 변경
        StartCoroutine(LoadScene(SceneResource.Instance.TitleRequireScenes, null, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects, startEffect, endEffect, screen));
    }

    public void LoadFieldScene(string loadMap, UnloadSceneOptions unloadOptions, SceneFadeEffect startEffect, SceneFadeEffect endEffect, LoadingScreen screen)
    {
        // 로딩 간에 현재 게임 상태를 필드로 변경하기
        loadingCallBack += () => GameData.Instance.State = GameState.Field;

        // 메인 씬 지정
        loadingCallBack += () => SceneManager.SetActiveScene(SceneManager.GetSceneByName(SceneResource.Instance.FieldMainScene));

        // 씬 변경
        StartCoroutine(LoadScene(SceneResource.Instance.FieldRequireScenes, loadMap, unloadOptions, startEffect, endEffect, screen));
    }

    public void LoadBattleScene(string loadMap, UnloadSceneOptions unloadOptions, SceneFadeEffect startEffect, SceneFadeEffect endEffect, LoadingScreen screen)
    {
        // 로딩 간에 현재 게임 상태를 전투로 변경하기
        loadingCallBack += () => GameData.Instance.State = GameState.Battle;

        // 메인 씬 지정
        loadingCallBack += () => SceneManager.SetActiveScene(SceneManager.GetSceneByName(SceneResource.Instance.BattleMainScene));

        // 씬 변경
        StartCoroutine(LoadScene(SceneResource.Instance.BattleRequireScenes, loadMap, unloadOptions, startEffect, endEffect, screen));
    }

    private IEnumerator LoadScene(List<string> requireScenes, string loadMap, UnloadSceneOptions unloadOptions, SceneFadeEffect startEffect, SceneFadeEffect endEffect, LoadingScreen screen)
    {
        List<string> activeScenes = FindActiveScenes();

        // 활성화된 씬과 필요한 씬 비교 후 각각 로드할 씬, 언로드할 씬 리스트 생성
        List<string> unloadScenes = activeScenes.Except(requireScenes).ToList();
        List<string> loadScenes = requireScenes.Except(activeScenes).ToList();

        // 같은 맵이어도 다시 로드하기 위해 활성화 할 목록에 추가
        if (!string.IsNullOrEmpty(loadMap)) loadScenes.Add(loadMap);

        // 로딩씬 불러오기
        yield return StartCoroutine(LoadSceneCoroutine(SceneResource.Instance.LoadingScene, LoadSceneMode.Additive));

        // 로딩 간에 실행될 함수 설정
        loadingCallBack += () => InitCallBack();
        loadingScreen.loadingCallBack = loadingCallBack;

        // 로딩화면 띄우기
        loadingScreen.EnableScreen(loadScenes, unloadScenes, unloadOptions, startEffect, endEffect, screen);
    }

    private IEnumerator LoadSceneCoroutine(string sceneName, LoadSceneMode mode)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, mode);
        if (asyncLoad == null) yield break;

        while (!asyncLoad.isDone)
        {
            // 씬 로드 완료될 때까지 대기
            yield return null;
        }
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