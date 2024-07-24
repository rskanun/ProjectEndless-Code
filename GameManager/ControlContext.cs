using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ControlContext : ScriptableObject
{
    // 저장 파일 위치
    private const string OPTION_FILE_DIRECTORY = "Assets/Resources";
    private const string FILE_DIRECTORY = "Assets/Resources/Option";
    private const string FILE_PATH = "Assets/Resources/Option/ControlContext.asset";

    private static ControlContext _instance;
    public static ControlContext Instance
    {
        get
        {
            if (_instance != null) return _instance;

            _instance = Resources.Load<ControlContext>("Option/ControlContext");

#if UNITY_EDITOR
            if (_instance == null)
            {
                // 파일 경로가 없을 경우 폴더 생성
                if (!AssetDatabase.IsValidFolder(FILE_DIRECTORY))
                {
                    if (!AssetDatabase.IsValidFolder(OPTION_FILE_DIRECTORY))
                    {
                        AssetDatabase.CreateFolder("Assets", "Resources");
                    }

                    AssetDatabase.CreateFolder(OPTION_FILE_DIRECTORY, "Option");
                }

                // Resource.Load가 실패했을 경우
                _instance = AssetDatabase.LoadAssetAtPath<ControlContext>(FILE_PATH);

                if (_instance == null)
                {
                    _instance = CreateInstance<ControlContext>();
                    AssetDatabase.CreateAsset(_instance, FILE_PATH);
                }
            }
#endif
            return _instance;
        }
    }

    private IControlState _currentState;
    public IControlState CurrentState
    {
        get { return _currentState; }
    }

    public bool NoKeyDown
    {
        set { _noKeyDown = value; }
    }
    private bool _noKeyDown;


    public void OnKeyPressed()
    {
        if (_noKeyDown == false)
            _currentState.OnControlKeyPressed();
    }

    public void SetState(IControlState state)
    {
        _currentState = state;
    }
}