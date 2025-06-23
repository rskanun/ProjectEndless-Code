using UnityEngine;

public class MapManager
{
    public static PolygonCollider2D CurrentArea { get; private set; }
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

    public static void SetCurrentArea(PolygonCollider2D collider)
    {
        CurrentArea = collider;

        // 구역 이동 알림
        GameEventManager.Instance.NotifyAreaChanged();
    }
}
