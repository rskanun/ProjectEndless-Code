using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerTrackerCamera : MonoBehaviour
{
    public Transform target;

    public float moveSpeed;

    // 카메라 최소 최대 위치 좌표
    private Vector2 minPos;
    private Vector2 maxPos;
    private Vector2 pos;
    private Vector2 size;

    public void RoomChanged()
    {
        MapAreaSet(FieldData.Instance.CurrentField);

        // 해당 이벤트의 위치를 캐릭터에 고정
        transform.position = target.position;
    }

    private void MapAreaSet(Tilemap map)
    {
        // 맵 사이즈 및 시작점 Vector2로 변환
        Vector2 mapSize = new Vector2(map.size.x , map.size.y);
        Vector2 mapOrigin = new Vector2(map.origin.x , map.origin.y);

        // 맵 중심 계산
        Vector2 mapPosition = mapOrigin + mapSize / 2;

        // 카메라 사이즈 계산
        Vector2 cameraSize = new Vector2(2 * Camera.main.orthographicSize * Camera.main.aspect, 2 * Camera.main.orthographicSize);

        // 카메라의 끝과 맵의 끝이 닿는 범위 계산
        Vector2 cameraMoveSize = (mapSize - cameraSize) / 2;

        minPos = mapPosition - cameraMoveSize;
        maxPos = mapPosition + cameraMoveSize;

        pos = mapPosition;
        size = minPos - maxPos;
    }

    private void LateUpdate()
    {
        // 범위 밖으로 나가지 않도록 해당 이벤트의 위치를 조정
        float blockX = Mathf.Clamp(target.position.x, minPos.x, maxPos.x);
        float blockY = Mathf.Clamp(target.position.y, minPos.y, maxPos.y);

        transform.position = Vector3.Lerp(transform.position, new Vector3(blockX, blockY, -1), moveSpeed);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(pos, size);
    }
}
