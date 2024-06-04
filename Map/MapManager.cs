using UnityEngine;

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
        LoadSceneManager.Instance.OnSceneClosed(map.SceneName);
    }
}
