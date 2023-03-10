using System.IO;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class OptionSetting : ScriptableObject
{
    // 저장 파일 위치
    private const string FILE_DIRECTORY = "Assets/Resources/Option";
    private const string FILE_PATH = "Assets/Resources/Option/OptionSetting.asset";

    private const string SAVE_FILE_PATH = "Assets/Resources/option.txt";

    private static OptionSetting _instance;
    public static OptionSetting Instance
    {
        get
        {
            if (_instance != null) return _instance;

            _instance = Resources.Load<OptionSetting>("OptionSetting");

#if UNITY_EDITOR
            if (_instance == null)
            {
                // 파일 경로가 없을 경우 폴더 생성
                if (!AssetDatabase.IsValidFolder(FILE_DIRECTORY))
                {
                    AssetDatabase.CreateFolder("Assets", "Resources");
                    AssetDatabase.CreateFolder("Resources", "Option");
                }

                // Resource.Load가 실패했을 경우
                _instance = AssetDatabase.LoadAssetAtPath<OptionSetting>(FILE_PATH);

                if (_instance == null)
                {
                    _instance = CreateInstance<OptionSetting>();
                    AssetDatabase.CreateAsset(_instance, FILE_PATH);
                }
            }
#endif

            return _instance;
        }
    }

    private bool isController = false;
    public bool IsController
    {
        get { return isController; }
        set { isController = value; }
    }

    // 컨트롤키
    public KeyCode Left { get { return left; } }
    private KeyCode left  = KeyCode.LeftArrow;
    public KeyCode Right { get { return right; } }
    private KeyCode right = KeyCode.RightArrow;
    public KeyCode Up { get { return up; } }
    private KeyCode up    = KeyCode.UpArrow;
    public KeyCode Down { get { return down; } }
    private KeyCode down  = KeyCode.DownArrow;

    // 액션키
    public KeyCode Action { get { return action; } }
    private KeyCode action    = KeyCode.Mouse0;
    public KeyCode Dash { get { return dash; } }
    private KeyCode dash      = KeyCode.Mouse1;
    public KeyCode Interact { get { return interact; } }
    private KeyCode interact  = KeyCode.E;

    // 선택키
    public KeyCode Select { get { return select; } }
    private KeyCode select    = KeyCode.Return;

    // 취소 및 돌아가기 키
    public KeyCode Cancel { get { return cancel; } }
    private KeyCode cancel    = KeyCode.Escape;

    // 옵션(ESC)키
    public KeyCode Menu { get { return menu; } }
    private KeyCode menu = KeyCode.Escape;

    // 스크립트 속도
    public float TypingSpeed { get { return typingSpeed; } }
    private float typingSpeed = 0.025f;
}
