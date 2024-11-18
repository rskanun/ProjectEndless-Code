using UnityEngine;
using UnityEngine.InputSystem;


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

    [SerializeField]
    private InputActionAsset inputAction;

    private IController _initController;
    public IController InitController
    {
        private set { _initController = value; }
        get { return _initController; }
    }

    private IController _currentController;
    public IController CurrentController
    {
        private set { _currentController = value; }
        get { return _currentController; }
    }

    private MainInput _keyInput;
    public MainInput KeyInput
    {
        get
        {
            if (_keyInput == null)
                _keyInput = new MainInput();

            return _keyInput;
        }
    }

    private bool _keyBlock;
    public bool KeyBlock
    {
        private set { _keyBlock = value; }
        get { return _keyBlock; }
    }

    public void Init()
    {
        // 변수 초기화
        KeyBlock = false;
    }

    public void SetInitController(IController controller)
    {
        _initController = controller;
    }

    public void ResetController()
    {
        if (_initController == null)
        {
            // 초기값이 정해져있지 않으면 실행X
            return;
        }

        // 초기 컨트롤러로 설정
        SetController(_initController);
    }

    public void SetController(IController controller)
    {
        // 기존 컨트롤러 연결 끊기
        CurrentController?.OnDisconnected();

        // 새 컨트롤러 연결
        CurrentController = controller;
        CurrentController?.OnConnected();
    }

    public void KeyLock()
    {
        KeyBlock = true;

        // 현재 연결된 컨트롤러 연결 끊기
        CurrentController.OnDisconnected();
    }

    public void KeyUnlock()
    {
        KeyBlock = false;

        // 현재 연결된 컨트롤러 다시 재연결
        CurrentController.OnConnected();
    }
}