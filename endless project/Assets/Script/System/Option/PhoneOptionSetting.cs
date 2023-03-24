using UnityEditor;
using UnityEngine;

namespace Assets.Script.System.Option
{
    public class PhoneOptionSetting : ScriptableObject
    {
        // 저장 파일 위치
        private const string OPTION_FILE_DIRECTORY = "Assets/Resources";
        private const string FILE_DIRECTORY = "Assets/Resources/Option";
        private const string FILE_PATH = "Assets/Resources/Option/PhoneOptionSetting.asset";

        private static PhoneOptionSetting _instance;
        public static PhoneOptionSetting Instance
        {
            get
            {
                if (_instance != null) return _instance;

                _instance = Resources.Load<PhoneOptionSetting>("Option/PhoneOptionSetting");

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
                    _instance = AssetDatabase.LoadAssetAtPath<PhoneOptionSetting>(FILE_PATH);

                    if (_instance == null)
                    {
                        _instance = CreateInstance<PhoneOptionSetting>();
                        AssetDatabase.CreateAsset(_instance, FILE_PATH);
                    }
                }
#endif

                return _instance;
            }
        }

        // WiFi
        [SerializeField]
        private bool network = false;
        public bool Network 
        { 
            get { return network; } 
            set { network = value; }
        }

        // 전파
        [SerializeField]
        private bool service = true;
        public bool Service 
        {
            get { return service; }
            set { service = value; }
        }
    }
}