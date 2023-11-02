using UnityEngine;
using System.Collections.Generic;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public class ChapterData
{
    public int chapterID;
    public TextAsset csvFile;
}

public class ChapterResource : ScriptableObject
{
    private const string FILE_DIRECTORY = "Assets/Resources/Scripts";
    private const string FILE_PATH = "Assets/Resources/Scripts/ChapterResource.asset";

    private static ChapterResource _instance;
    public static ChapterResource Instance
    {
        get
        {
            if (_instance != null) return _instance;

            _instance = Resources.Load<ChapterResource>("Scripts/ChapterResource");

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

                    AssetDatabase.CreateFolder("Assets/Resources", "Scripts");
                }

                // Resource.Load가 실패했을 경우
                _instance = AssetDatabase.LoadAssetAtPath<ChapterResource>(FILE_PATH);

                if (_instance == null)
                {
                    _instance = CreateInstance<ChapterResource>();
                    AssetDatabase.CreateAsset(_instance, FILE_PATH);
                }
            }
#endif

            return _instance;
        }
    }

    [SerializeField]
    private List<ChapterData> _data;
    public List<ChapterData> Data { get { return _data; } }
}
