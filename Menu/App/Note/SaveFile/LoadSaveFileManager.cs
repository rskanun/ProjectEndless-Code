using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSaveFileManager : MonoBehaviour
{
    private SaveData loadData;

    [Header("참조 스크립트")]
    [SerializeField] private LoadManager loadManager;

    public void LoadSaveFile(SaveData data)
    {
        loadData = data;
        SceneManager.sceneLoaded += LoadData;

        // 씬 이동
        MapManager.LoadMap(data.mapData.id);
    }

    private void LoadData(Scene scene, LoadSceneMode mode)
    {
        if (loadData != null)
        {
            loadManager.LoadGameData(loadData);
            SceneManager.sceneLoaded -= LoadData;
        }
    }
}