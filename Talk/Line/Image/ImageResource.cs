using UnityEngine;
using Sirenix.OdinInspector;
using System.IO;
using System.Collections.Generic;
using System;
using System.Linq;


#if UNITY_EDITOR
using UnityEditor;
#endif

public class ImageResource : ScriptableObject
{
    // 저장 파일 위치
    private const string FILE_DIRECTORY = "Assets/Resources/Option";
    private const string FILE_PATH = "Assets/Resources/Option/ImageResource.asset";

    private static ImageResource _instance;
    public static ImageResource Instance
    {
        get
        {
            if (_instance != null) return _instance;

            _instance = Resources.Load<ImageResource>("Option/ImageResource");

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
                _instance = AssetDatabase.LoadAssetAtPath<ImageResource>(FILE_PATH);
                if (_instance == null)
                {
                    _instance = CreateInstance<ImageResource>();
                    AssetDatabase.CreateAsset(_instance, FILE_PATH);
                }
            }
#endif
            return _instance;
        }
    }

    [SerializeField]
    [FolderPath(RequireExistingPath = true)]
    private string folderPath;
    private string[] extensions = { ".png", ".jpg", ".jpeg" };

#if UNITY_EDITOR
    public Sprite FindSpriteFile(string fileName)
    {
        // 이미지를 찾을 폴더가 할당되어 있지 않거나, 찾을 수 없다면 실행 중단
        if (string.IsNullOrEmpty(folderPath) || !AssetDatabase.IsValidFolder(folderPath))
        {
            throw new InvalidOperationException("폴더 주소가 할당되어 있지 않습니다.");
        }

        // folderPath를 기준으로 모든 Sprite 에셋의 guid 가져오기
        string[] guids = AssetDatabase.FindAssets($"{fileName} t:Sprite", new[] { folderPath });

        foreach (string guid in guids)
        {
            // 에셋의 guid를 토대로 주소 가져오기
            string path = AssetDatabase.GUIDToAssetPath(guid);

            // 이름과 확장자 체크
            bool checkToName = Path.GetFileNameWithoutExtension(path) == fileName;
            bool checkToExtension = extensions.Contains(Path.GetExtension(path));
            if (checkToName && checkToExtension)
            {
                // 이름과 확장자를 체크하여 일치하면 리턴
                return AssetDatabase.LoadAssetAtPath<Sprite>(path);
            }
        }

        Debug.LogWarning($"허용된 확장자를 가진 {fileName} 파일을 해당 폴더 내에서 찾지 못했습니다.");
        return null;
    }
#endif
}