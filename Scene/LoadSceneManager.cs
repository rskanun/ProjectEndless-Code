using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;



#if UNITY_EDITOR
using UnityEditor;
#endif

public enum LoadAnimationType
{
    BlurFadeIn,
    BlurFadeOut,
    Loading,
    TimePassLoading
}

public class LoadSceneManager : ScriptableObject
{
    // 저장 파일 위치
    private const string FILE_DIRECTORY = "Assets/Resources";
    private const string FILE_PATH = "Assets/Resources/LoadSceneManager.asset";

    private static LoadSceneManager _instance;
    public static LoadSceneManager Instance
    {
        get
        {
            if (_instance != null) return _instance;
            _instance = Resources.Load<LoadSceneManager>("LoadSceneManager");

#if UNITY_EDITOR
            if (_instance == null)
            {
                // 파일 경로가 없을 경우 폴더 생성
                if (!AssetDatabase.IsValidFolder(FILE_DIRECTORY))
                {
                    string[] folders = FILE_DIRECTORY.Split('/');
                    string currentPath = folders[0];

                    for (int i = 1; i < folders.Length; i++)
                    {
                        if (!AssetDatabase.IsValidFolder(currentPath + "/" + folders[i]))
                        {
                            AssetDatabase.CreateFolder(currentPath, folders[i]);
                        }
                        currentPath += "/" + folders[i];
                    }
                }

                // Resource.Load가 실패했을 경우
                _instance = AssetDatabase.LoadAssetAtPath<LoadSceneManager>(FILE_PATH);
                if (_instance == null)
                {
                    _instance = CreateInstance<LoadSceneManager>();
                    AssetDatabase.CreateAsset(_instance, FILE_PATH);
                }
            }
#endif
            return _instance;
        }
    }

    [SerializeField]
    private SceneAsset loadingScene;

    [SerializeField]
    private List<SceneAsset> fieldRequireSceneAssets;
    private List<string> fieldRequireScenes = new List<string>();

    [SerializeField]
    private List<SceneAsset> battleRequireSceneAssets;
    private List<string> battleRequireScenes = new List<string>();

    [SerializeField]
    private List<SceneAsset> titleRequireSceneAssets;
    private List<string> titleRequireScenes = new List<string>();

    private SceneAnimationManager animationManager;

#if UNITY_EDITOR
    private void OnValidate()
    {
        fieldRequireScenes = SceneAssetsToString(fieldRequireSceneAssets);
        battleRequireScenes = SceneAssetsToString(battleRequireSceneAssets);
        titleRequireScenes = SceneAssetsToString(titleRequireSceneAssets);
    }

    private List<string> SceneAssetsToString(List<SceneAsset> sceneAssets)
    {
        // Scene Asset 리스트에서 이름만 추출하여 내보내기
        return sceneAssets.Select(scene => scene.name).ToList();
    }
#endif

    public void RegisterManager(SceneAnimationManager manager)
    {
        animationManager = manager;
    }

    public void RemoveManager()
    {
        animationManager = null;
    }

    /************************************************************
     * [씬 전환]
     * 
     * 상황에 따른 씬 전환 시 띄울 애니메이션과 활성화 할 씬 관리
     ************************************************************/

    public void LoadTitleSceneToField()
    {
        // 타이틀에서 게임을 시작하고자 필드 씬을 불러올 때
    }

    public void LoadFieldSceneToTitle(string loadMap)
    {
        // 필드에서 게임을 종료하고자 타이틀 씬을 불러올 때
    }

    public void LoadFieldSceneToPast(string loadMap)
    {
        // 필드에서 동일한 세계의 다른 데이터를 가져오고자 새로운 필드 씬을 불러올 때
    }

    public void LoadFieldSceneToParallel(string loadMap)
    {
        // 필드에서 다른 평행 세계의 데이터를 가져오고자 새로운 필드 씬을 불러올 때
    }

    public void LoadBattleSceneToField(string loadMap)
    {
        // 필드에서 전투 돌입 시 전투 씬을 불러올 때
    }

    public void LoadFieldSceneToBattle(string loadMap)
    {
        // 전투에서 한 쪽의 승리로 끝나 다시 본래있던 필드 씬을 불러올 때
    }

    private void LoadTitleScene(LoadAnimationType startAnimation, LoadAnimationType loadAnimation, LoadAnimationType endAnimation)
    {
        LoadScene(titleRequireScenes, startAnimation, loadAnimation, endAnimation);
    }

    private void LoadFieldScene(string loadMap, LoadAnimationType startAnimation, LoadAnimationType loadAnimation, LoadAnimationType endAnimation)
    {
        List<string> requireScenes = fieldRequireScenes.Append(loadMap).ToList();
        LoadScene(requireScenes, startAnimation, loadAnimation, endAnimation);
    }

    private void LoadBattleScene(string loadMap, LoadAnimationType startAnimation, LoadAnimationType loadAnimation, LoadAnimationType endAnimation)
    {
        List<string> requireScenes = battleRequireScenes.Append(loadMap).ToList();
        LoadScene(requireScenes, startAnimation, loadAnimation, endAnimation);
    }

    private void LoadScene(List<string> requireScenes, LoadAnimationType startAnimation, LoadAnimationType loadAnimation, LoadAnimationType endAnimation)
    {
        // 로딩씬 불러오기
        // -> 로딩씬 전환 애니메이션 띄우기
        // -> 종료 시 로딩화면 띄우기
        // -> 필요없는 씬 제거
        // -> 제거 후 필요한 씬 불러오기
        // -> 모든 씬을 불러왔다면 씬제거 애니메이션 띄우기

        List<string> activeScenes = FindActiveScenes();

        // 현재 활성화되어 있는 씬 중에서 제거할 씬 찾기
        List<string> unloadScenes = activeScenes
            .Where(scene => !requireScenes.Contains(scene))
            .ToList();

        // 활성화해야 할 씬 중에서 현재 활성화되어 있지 않는 씬 찾기
        List<string> loadScenes = requireScenes
            .Where(scene => !activeScenes.Contains(scene))
            .ToList();

        // 로딩씬 불러오기
        SceneManager.LoadSceneAsync(loadingScene.name, LoadSceneMode.Additive);


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