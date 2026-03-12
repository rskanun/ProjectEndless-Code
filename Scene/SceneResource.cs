using UnityEngine;
using System.Collections.Generic;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;

[System.Serializable]
public class SceneConfiguration
{
    [SerializeField]
    private SceneAsset _mainSceneAsset;
    public SceneAsset MainScene => _mainSceneAsset;

    [SerializeField]
    private List<SceneAsset> _requireSceneAssets;
    public List<SceneAsset> RequireScenes => _requireSceneAssets;
}
#endif

public class SceneResource : ScriptableObject
{
    // 저장 파일 위치
    private const string FILE_DIRECTORY = "Assets/Resources/Option";
    private const string FILE_PATH = "Assets/Resources/Option/SceneResource.asset";

    private static SceneResource _instance;
    public static SceneResource Instance
    {
        get
        {
            if (_instance != null) return _instance;

            _instance = Resources.Load<SceneResource>("Option/SceneResource");

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
                _instance = AssetDatabase.LoadAssetAtPath<SceneResource>(FILE_PATH);
                if (_instance == null)
                {
                    _instance = CreateInstance<SceneResource>();
                    AssetDatabase.CreateAsset(_instance, FILE_PATH);
                }
            }
#endif
            return _instance;
        }
    }

    [Header("메인 씬")]
    [SerializeField]
    private SceneAsset _mainSceneAsset;
    private string _mainScene;
    public string MainScene => _mainScene;

    [Header("로딩 씬")]
    [SerializeField]
    private SceneAsset _loadingSceneAsset;
    private string _loadingScene;
    public string LoadingScene => _loadingScene;

    [Header("상태별 씬 설정")]
    [SerializeField]
    private SceneConfiguration _fieldSceneConfig;
    private string _fieldMainScene;
    public string FieldMainScene => _fieldMainScene;
    private List<string> _fieldRequireScenes;
    public List<string> FieldRequireScenes => _fieldRequireScenes;

    [SerializeField]
    private SceneConfiguration _battleSceneConfig;
    private string _battleMainScene;
    public string BattleMainScene => _battleMainScene;
    private List<string> _battleRequireScenes;
    public List<string> BattleRequireScenes => _battleRequireScenes;

    [SerializeField]
    private SceneConfiguration _titleSceneConfig;
    private string _titleMainScene;
    public string TitleMainScene => _titleMainScene;
    private List<string> _titleRequireScenes;
    public List<string> TitleRequireScenes => _titleRequireScenes;

#if UNITY_EDITOR
    private void OnValidate()
    {
        // 메인 씬
        if (_mainSceneAsset != null)
        {
            _mainScene = _mainSceneAsset.name;
        }

        // 로딩 씬
        if (_loadingSceneAsset != null)
        {
            _loadingScene = _loadingSceneAsset.name;
        }

        // 씬 설정
        if (_fieldSceneConfig.MainScene != null)
        {
            _fieldMainScene = _fieldSceneConfig.MainScene.name;
            _fieldRequireScenes = SceneAssetsToString(_fieldSceneConfig.RequireScenes);
        }

        if (_battleSceneConfig.MainScene != null)
        {
            _battleMainScene = _battleSceneConfig.MainScene.name;
            _battleRequireScenes = SceneAssetsToString(_battleSceneConfig.RequireScenes);
        }

        if (_titleSceneConfig.MainScene != null)
        {
            _titleMainScene = _titleSceneConfig.MainScene.name;
            _titleRequireScenes = SceneAssetsToString(_titleSceneConfig.RequireScenes);
        }
    }

    private List<string> SceneAssetsToString(List<SceneAsset> sceneAssets)
    {
        // Scene Asset 리스트에서 이름만 추출하여 내보내기
        return sceneAssets
            .Where(scene => scene != null)
            .Select(scene => scene.name)
            .ToList();
    }
#endif
}