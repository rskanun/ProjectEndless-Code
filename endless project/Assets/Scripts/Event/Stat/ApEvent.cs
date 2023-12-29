using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ApEvent : GameEvent
{
    private const string FILE_DIRECTORY = "Assets/Resources/Events";
    private const string FILE_PATH = "Assets/Resources/Events/ApEvent.asset";

    private static ApEvent _instance;
    public static ApEvent Instance
    {
        get
        {
            if (_instance != null) return _instance;

            _instance = Resources.Load<ApEvent>("Events/ApEvent");

#if UNITY_EDITOR
            if (_instance == null)
            {
                // 파일 경로가 없을 경우 폴더 생성
                if (!AssetDatabase.IsValidFolder(FILE_DIRECTORY))
                {
                    if (!AssetDatabase.IsValidFolder("Assets/Resources"))
                    {
                        AssetDatabase.CreateFolder("Assets", "Resources");
                    }

                    AssetDatabase.CreateFolder("Assets/Resources", "Events");
                }

                // Resource.Load가 실패했을 경우
                _instance = AssetDatabase.LoadAssetAtPath<ApEvent>(FILE_PATH);

                if (_instance == null)
                {
                    _instance = CreateInstance<ApEvent>();
                    AssetDatabase.CreateAsset(_instance, FILE_PATH);
                }
            }
#endif

            return _instance;
        }
    }

    [MenuItem("GameObject/Singleton Scriptable Object/ApEvent", false, 30)]
    public static void CreateInInspector()
    {
        ApEvent instance = Instance;
    }
}