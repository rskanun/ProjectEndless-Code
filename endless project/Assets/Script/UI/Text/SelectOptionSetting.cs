using UnityEditor;
using UnityEngine;

[System.Serializable]
public class SelectButton
{
    public float width;
    public float height;
}

[System.Serializable]
public class SelectContainer
{
    public float width;
    public float height;
    public float spacing;
}

[CreateAssetMenu(menuName = "Singleton Object/SelectOptionSetting", fileName = "SelectOptionSetting")]
public class SelectOptionSetting : ScriptableObject
{
    // 저장 파일 위치
    private const string FILE_DIRECTORY = "Assets/Resources/Option";
    private const string FILE_PATH = "Assets/Resources/Option/SelectOptionSetting.asset";

    private static SelectOptionSetting _instance;
    public static SelectOptionSetting Instance
    {
        get
        {
            if (_instance != null) return _instance;

            _instance = Resources.Load<SelectOptionSetting>("Option/SelectOptionSetting");

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
                _instance = AssetDatabase.LoadAssetAtPath<SelectOptionSetting>(FILE_PATH);

                if (_instance == null)
                {
                    _instance = CreateInstance<SelectOptionSetting>();
                    AssetDatabase.CreateAsset(_instance, FILE_PATH);
                }
            }
#endif

            return _instance;
        }
    }

    [SerializeField]
    private SelectButton _buttonSetting;
    public SelectButton ButtonSetting { get { return _buttonSetting; } }

    [SerializeField]
    private SelectContainer _containerSetting;
    public SelectContainer ContainerSetting { get { return _containerSetting; } }
}