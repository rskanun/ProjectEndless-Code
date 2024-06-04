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

    // 체력
    public int HP { get { return _playerData.HP; } }
    public int MaxHP { get { return _playerData.MaxHP; } }

    // 근력
    public int STR { get { return _playerData.STR; } }

    // 민첩
    public int AGI { get { return _playerData.AGI; } }

    // 이동속도
    public int MoveSpeed { get { return _playerData.MoveSpeed; } }
    public float DashSpeed { get { return _playerData.DashSpeed; } }

    // 마나
    public int Mana { get { return _playerData.Mana; } }
    public int MaxMana { get { return _playerData.MaxMana; } }

    // 마력
    public int MP { get { return _playerData.MP; } }

    // 각성치
    public int AP { get { return _playerData.AP;} }
    public int MaxAP { get { return _playerData.MaxAP; } }

    // 방어력
    public int DEF { get { return _playerData.DEF;} }

    // 스테미나
    public int Stamina { get { return _playerData.Stamina;} }
    public int MaxStamina { get { return _playerData.MaxStamina; } }

    // 현재 위치
    public Vector2 Position { get { return _playerData.Position;} }
}