using System.Collections;
using UnityEditor;
using UnityEngine;

public class OptionData : ScriptableObject
{
    // 저장 파일 위치
    private const string OPTION_FILE_DIRECTORY = "Assets/Resources";
    private const string FILE_DIRECTORY = "Assets/Resources/Option";
    private const string FILE_PATH = "Assets/Resources/Option/OptionData.asset";

    private static OptionData _instance;
    public static OptionData Instance
    {
        get
        {
            if (_instance != null) return _instance;

            _instance = Resources.Load<OptionData>("Option/OptionData");

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
                _instance = AssetDatabase.LoadAssetAtPath<OptionData>(FILE_PATH);

                if (_instance == null)
                {
                    _instance = CreateInstance<OptionData>();
                    AssetDatabase.CreateAsset(_instance, FILE_PATH);
                }
            }
#endif
            return _instance;
        }
    }

    /************************************************************
    * [옵션 데이터]
    * 
    * 게임 설정과 관련된 데이터
    ************************************************************/

    [SerializeField]
    private float _typingSpeed = 0.025f;
    public float TypingSpeed
    {
        get { return _typingSpeed; }
    }



    /************************************************************
    * [휴식 데이터]
    * 
    * 휴식 시간에 따른 회복량 관련 데이터
    ************************************************************/
    [SerializeField]
    private int _rotaryRegenHP;
    public int RotaryRegenHP
    {
        get => _rotaryRegenHP;
        set => _rotaryRegenHP = Mathf.Clamp(value, 0, 100);
    }

    [SerializeField]
    private int _rotaryRegenSP;
    public int RotaryRegenSP
    {
        get => _rotaryRegenSP;
        set => _rotaryRegenSP = Mathf.Clamp(value, 0, 100);
    }
}