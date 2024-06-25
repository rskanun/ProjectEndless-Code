using UnityEditor;
using UnityEngine;

public class ReadOnlyPlayerData : ScriptableObject
{
    // 저장 파일 위치
    private const string FILE_DIRECTORY = "Assets/Resources/Object/Character/Player";
    private const string FILE_PATH = "Assets/Resources/Object/Character/Player/ReadOnlyPlayerData.asset";

    private static ReadOnlyPlayerData _instance;
    public static ReadOnlyPlayerData Instance
    {
        get
        {
            if (_instance != null) return _instance;

            _instance = Resources.Load<ReadOnlyPlayerData>("Object/Character/Player/ReadOnlyPlayerData");

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
                _instance = AssetDatabase.LoadAssetAtPath<ReadOnlyPlayerData>(FILE_PATH);

                if (_instance == null)
                {
                    _instance = CreateInstance<ReadOnlyPlayerData>();
                    AssetDatabase.CreateAsset(_instance, FILE_PATH);

                    // Player.asset 불러옴
                    PlayerData playerData = AssetDatabase.LoadAssetAtPath<PlayerData>(FILE_DIRECTORY + "/Player.asset");

                    if (playerData == null)
                    {
                        playerData = CreateInstance<PlayerData>();
                        AssetDatabase.CreateAsset(playerData, FILE_DIRECTORY + "/Player.asset");
                    }

                    _instance._playerData = playerData;
                }
            }
#endif
            return _instance;
        }
    }

    [SerializeField]
    private PlayerData _playerData;

    // 각성치
    public int AP { get { return _playerData.AP;} }
    public int MaxAP { get { return _playerData.MaxAP; } }

    // 현재 위치
    public Vector2 Position { get { return _playerData.Position;} }
}