using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Assets.Script.System
{
    public class NoKeyDown : ScriptableObject
    {
        private const string FILE_DIRECTORY = "Assets/Resources/Option";
        private const string FILE_PATH = "Assets/Resources/Option/NoKeyDown.asset";

        private static NoKeyDown _instance;
        public static NoKeyDown Instance
        {
            get
            {
                if (_instance != null) return _instance;

                _instance = Resources.Load<NoKeyDown>("NoKeyDown");

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
                    _instance = AssetDatabase.LoadAssetAtPath<NoKeyDown>(FILE_PATH);

                    if (_instance == null)
                    {
                        _instance = CreateInstance<NoKeyDown>();
                        AssetDatabase.CreateAsset(_instance, FILE_PATH);
                    }
                }
#endif

                return _instance;
            }
        }

        // 플레이어가 현재 대시를 하고 있는 상태인지 여부
        private bool isDashing = false;
        public bool IsDashing
        {
            get { return isDashing; }
            set { isDashing = value; }
        }

        // 현재 플레이어가 NPC와 대화를 진행중인 상태인지 여부
        private bool isTalking = false;
        public bool IsTalking
        {
            get { return isTalking; }
            set { isTalking = value; }
        }

        // 메뉴 화면이 현재 켜져있는지 여부
        private bool isMenuActive = false;
        public bool IsMenuActive
        {
            get { return isMenuActive; }
            set { isMenuActive = value; }
        }

        // 플레이어를 조종할 수 있는지 여부
        private bool isPlayerControllable = true;
        public bool IsPlayerControllable
        {
            get
            {
                if (isPlayerControllable)
                    return isDashing == false && isTalking == false && isMenuActive == false;
                else
                    return isPlayerControllable;
            }

            set { isPlayerControllable = value; }
        }

        private bool isMenuOpenable = true;
        public bool IsMenuOpenable
        {
            get
            {
                if (isMenuOpenable)
                    return isTalking == false;
                else
                    return isMenuActive;
            }

            set { isMenuOpenable = value; }
        }
    }
}