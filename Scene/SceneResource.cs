using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;




#if UNITY_EDITOR
using UnityEditor;
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
    private SceneAsset _mainScene;
    public string MainScene => _mainScene.name;

    [Header("로딩 씬")]
    [SerializeField]
    private SceneAsset _loadingScene;
    public string LoadingScene => _loadingScene.name;

    [Header("필수 씬 리스트")]
    [SerializeField]
    private List<SceneAsset> fieldRequireSceneAssets;
    private List<string> _fieldRequireScenes;
    public List<string> FieldRequireScenes
    {
        get
        {
            if (_fieldRequireScenes == null)
                _fieldRequireScenes = SceneAssetsToString(fieldRequireSceneAssets);

            return _fieldRequireScenes;
        }
    }

    [SerializeField]
    private List<SceneAsset> battleRequireSceneAssets;
    private List<string> _battleRequireScenes;
    public List<string> BattleRequireScenes
    {
        get
        {
            if (_battleRequireScenes == null)
                _battleRequireScenes = SceneAssetsToString(battleRequireSceneAssets);

            return _battleRequireScenes;
        }
    }

    [SerializeField]
    private List<SceneAsset> titleRequireSceneAssets;
    private List<string> _titleRequireScenes;
    public List<string> TitleRequireScenes
    {
        get
        {
            if (_titleRequireScenes == null)
                _titleRequireScenes = SceneAssetsToString(titleRequireSceneAssets);

            return _titleRequireScenes;
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
}