using UnityEngine;
using UnityEngine.SceneManagement;

public class MapManager
{
    public static MapData FindMap(string id)
    {
        MapData[] mapDataArray = Resources.LoadAll<MapData>("Map");

        foreach (MapData mapData in mapDataArray)
        {
            if (mapData.ID.Equals(id))
            {
                return mapData;
            }
        }

        return null;
    }

    public static void LoadMap(MapData map)
    {
        SceneManager.LoadScene(map.SceneName);
    }
}
