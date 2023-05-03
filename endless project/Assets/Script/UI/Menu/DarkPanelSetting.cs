using Assets.Script.System.Option;
using UnityEditor;
using UnityEngine;

public class DarkPanelSetting : ScriptableObject
{
    private const string FILE_DIRECTORY = "Assets/Resources/Option";
    private const string FILE_PATH = "Assets/Resources/Option/DarkPanelSetting.asset";

    private static DarkPanelSetting _instance;
    public static DarkPanelSetting Instance
    {
        get
        {
            if (_instance != null) return _instance;

            _instance = Resources.Load<DarkPanelSetting>("Option/DarkPanelSetting");

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
                    AssetDatabase.CreateFolder("Assets/Resources", "Option");
                }

                // Resource.Load가 실패했을 경우
                _instance = AssetDatabase.LoadAssetAtPath<DarkPanelSetting>(FILE_PATH);

                if (_instance == null)
                {
                    _instance = CreateInstance<DarkPanelSetting>();
                    AssetDatabase.CreateAsset(_instance, FILE_PATH);
                }
            }
#endif

            return _instance;
        }
    }

    [SerializeField]
    private Color _color;
    public Color PanelColor
    {
        get { return _color; }
    }
}