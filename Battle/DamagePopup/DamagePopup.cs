using UnityEngine;
using TMPro;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class DamagePopup : ScriptableObject
{
    // 저장 파일 위치
    private const string FILE_DIRECTORY = "Assets/Resources/Option";
    private const string FILE_PATH = "Assets/Resources/Option/DamagePopup.asset";

    private static DamagePopup _instance;
    public static DamagePopup Instance
    {
        get
        {
            if (_instance != null) return _instance;

            _instance = Resources.Load<DamagePopup>("Option/DamagePopup");

#if UNITY_EDITOR
            if (_instance == null)
            {
                // 파일 경로가 없을 경우 폴더 생성
                if (!AssetDatabase.IsValidFolder(FILE_DIRECTORY))
                {
                    string[] folders = FILE_DIRECTORY.Split('/');
                    string currentPath = folders[0];

                    for (int i = 1; i < folders.Length; i++)
                    {
                        if (!AssetDatabase.IsValidFolder(currentPath + "/" + folders[i]))
                        {
                            AssetDatabase.CreateFolder(currentPath, folders[i]);
                        }
                        currentPath += "/" + folders[i];
                    }
                }

                // Resource.Load가 실패했을 경우
                _instance = AssetDatabase.LoadAssetAtPath<DamagePopup>(FILE_PATH);
                if (_instance == null)
                {
                    _instance = CreateInstance<DamagePopup>();
                    AssetDatabase.CreateAsset(_instance, FILE_PATH);
                }
            }
#endif
            return _instance;
        }
    }

    [SerializeField] private GameObject damageObj;
    [SerializeField] private TMP_FontAsset fontAsset;
    [SerializeField] private int fontSize = 10;

    private DamageIndicator indicator;
    public GameObject DamagePrefab => damageObj;

#if UNITY_EDITOR
    private void OnValidate()
    {
        // 폰트 사이즈 1이상 고정
        InitFontSize();

        // 옵션 변경사항 반영
        SetFontOption(fontAsset, fontSize);
    }

    private void InitFontSize()
    {
        if (0 >= fontSize) fontSize = 1;
    }

    private void SetFontOption(TMP_FontAsset font, int size)
    {
        if (damageObj == null || font == null) return;

        TextMeshProUGUI text = damageObj.GetComponent<TextMeshProUGUI>();

        text.font = font;
        text.fontSize = size;
    }
#endif

    public static void IndicateDamage(Vector2 position, int damage)
    {
        Instance.indicator.IndicateDamage(position, damage);
    }

    public void RegisterIndicator(DamageIndicator indicator)
    {
        this.indicator = indicator;
    }

    public void RemoveIndicator()
    {
        indicator = null;
    }
}