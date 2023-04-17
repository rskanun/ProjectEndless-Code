using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class OptionSetting : ScriptableObject
{
    // 저장 파일 위치
    private const string OPTION_FILE_DIRECTORY = "Assets/Resources";
    private const string FILE_DIRECTORY = "Assets/Resources/Option";
    private const string FILE_PATH = "Assets/Resources/Option/OptionSetting.asset";

    private static OptionSetting _instance;
    public static OptionSetting Instance
    {
        get
        {
            if (_instance != null) return _instance;

            _instance = Resources.Load<OptionSetting>("Option/OptionSetting");

#if UNITY_EDITOR
            if (_instance == null)
            {
                // 파일 경로가 없을 경우 폴더 생성
                if (!AssetDatabase.IsValidFolder(FILE_DIRECTORY))
                {
                    if(!AssetDatabase.IsValidFolder(OPTION_FILE_DIRECTORY))
                    {
                        AssetDatabase.CreateFolder("Assets", "Resources");
                    }

                    AssetDatabase.CreateFolder("Assets/Resources", "Option");
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

    /************************************************************
    * [키 세팅]
    * 
    * 키 관련 변수
    ************************************************************/

    private bool isController = false;
    public bool IsController
    {
        get { return isController; }
        set { isController = value; }
    }

    // 컨트롤키
    public KeyCode Left { get { return left; } }
    private KeyCode left  = KeyCode.A;
    public KeyCode Right { get { return right; } }
    private KeyCode right = KeyCode.D;
    public KeyCode Up { get { return up; } }
    private KeyCode up    = KeyCode.W;
    public KeyCode Down { get { return down; } }
    private KeyCode down  = KeyCode.S;

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

    /************************************************************
    * [■■]
    * 
    * ■■ 횟수 관련 변수
    ************************************************************/
    private const int MAX_TIME = 24 * 60 * 60;
    private int _time = 30227;
    private int Time
    {
        set
        {
            if (value < 0) _time = MAX_TIME;
            else _time = value;
        }
    }

    public int Hour
    {
        get { return _time / 60 / 60; }
    }

    public int Minute
    {
        get { return _time / 60 % 60; }
    }

    public int Second
    {
        get { return _time % 60; }
    }

    public void timeSub()
    {
        _time -= 1;
    }

    /************************************************************
    * [기타]
    * 
    * 기타 옵션 관련 변수
    ************************************************************/

    // 스크립트 속도
    public float TypingSpeed { get { return _typingSpeed; } }
    private float _typingSpeed = 0.025f;

    /************************************************************
    * [세이브 로드]
    * 
    * 옵션 변수 세이브 로드 관련 함수
    ************************************************************/

    public void save()
    {
        PlayerPrefs.SetInt("time", _time);
        PlayerPrefs.SetFloat("typing speed", _typingSpeed);
    }

    public void load()
    {
        _time = PlayerPrefs.GetInt("time");
        _typingSpeed = PlayerPrefs.GetFloat("typing speed");
    }
}
