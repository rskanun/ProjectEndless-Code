using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class PlayerState : ScriptableObject
{
    private const string OPTION_FILE_DIRECTORY = "Assets/Resources";
    private const string FILE_DIRECTORY = "Assets/Resources/Option";
    private const string FILE_PATH = "Assets/Resources/Option/PlayerState.asset";

    private static PlayerState _instance;
    public static PlayerState Instance
    {
        get
        {
            if (_instance != null) return _instance;

            _instance = Resources.Load<PlayerState>("Option/PlayerState");

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

                    AssetDatabase.CreateFolder("Assets/Resources", "Option");
                }

                // Resource.Load가 실패했을 경우
                _instance = AssetDatabase.LoadAssetAtPath<PlayerState>(FILE_PATH);

                if (_instance == null)
                {
                    _instance = CreateInstance<PlayerState>();
                    AssetDatabase.CreateAsset(_instance, FILE_PATH);
                }
            }
#endif
            _instance.initialize();
            return _instance;
        }
    }

    // 플레이어가 현재 대시를 하고 있는 상태인지 여부
    private bool _isDashing;
    public bool IsDashing
    {
        get { return _isDashing; }
        set { _isDashing = value; }
    }

    // 현재 플레이어가 NPC와 대화를 진행중인 상태인지 여부
    private bool _isTalking;
    public bool IsTalking
    {
        get { return _isTalking; }
        set { _isTalking = value; }
    }

    // 메뉴 화면이 현재 켜져있는지 여부
    private bool _isMenuActive;
    public bool IsMenuActive
    {
        get { return _isMenuActive; }
        set { _isMenuActive = value; }
    }

    // 플레이어를 조종할 수 있는지 여부
    private bool _isPlayerControllable;
    public bool IsPlayerControllable
    {
        get
        {
            if (_isPlayerControllable)
                return _isDashing == false && _isTalking == false && _isMenuActive == false;
            else
                return _isPlayerControllable;
        }

        set { _isPlayerControllable = value; }
    }

    // 메뉴키를 누를 수 있는지 여부
    private bool _allowMenuKey;
    public bool AllowMenuKey
    {
        get
        {
            if (_allowMenuKey)
                return _isTalking == false;
            else
                return false;
        }

        set { _allowMenuKey = value; }
    }

    // 뒤로가기 키를 누를 수 있는지 여부
    private bool _allowCancelKey;
    public bool AllowCancelKey
    {
        get
        {
            if (_isMenuActive)
                return _allowMenuKey;

            return _allowCancelKey;
        }

        set { _allowCancelKey = value; }
    }

    public void initialize()
    {
        // init value
        _isDashing = false;
        _isTalking = false;
        _isMenuActive = false;

        _isPlayerControllable = true;
        _allowMenuKey = true;
    }
}