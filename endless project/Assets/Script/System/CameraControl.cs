using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform target;
    public GameObject mapArea;

    public float speed;

    private Vector2 min;
    private Vector2 max;

    void Start()
    {
        // 해당 이벤트의 위치를 캐릭터에 고정
        transform.position = target.position;

        // 카메라가 이동할 맵의 범위를 파악
        mapAreaSet();
    }

    void mapAreaSet()
    {
        // 맵의 가로 세로를 가져옴
        float mapWidth = mapArea.GetComponent<RectTransform>().rect.width;
        float mapHeight = mapArea.GetComponent<RectTransform>().rect.height;

        // 카메라의 가로 세로 및 위치도 가져옴
        float cameraHeight = 2 * Camera.main.orthographicSize;
        float cameraWidth = cameraHeight * Camera.main.aspect;

        Vector2 mapPotision = mapArea.transform.localPosition;

        // 카메라의 끝과 맵의 끝이 닿는 범위를 계산
        float distanceWidth = Mathf.Abs(mapWidth / 2 - cameraWidth / 2);
        float distanceHeight = Mathf.Abs(mapHeight / 2 - cameraHeight / 2);

        min = new Vector2(mapPotision.x - distanceWidth, mapPotision.y - distanceHeight);
        max = new Vector2(mapPotision.x + distanceWidth, mapPotision.y + distanceHeight);
    }

    void LateUpdate()
    {
        // 범위 밖으로 나가지 않도록 해당 이벤트의 위치를 조정
        float blockX = Mathf.Clamp(target.position.x, min.x, max.x);
        float blockY = Mathf.Clamp(target.position.y, min.y, max.y);

        transform.position = Vector2.Lerp(transform.position, new Vector2(blockX, blockY), speed);
    }
}
