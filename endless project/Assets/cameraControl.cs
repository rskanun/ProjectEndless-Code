using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraControl : MonoBehaviour
{
    public Transform target;

    public GameObject mapArea;

    public float speed;

    private Vector2 min;
    private Vector2 max;

    void Start()
    {
        transform.position = target.position;

        mapAreaSet();
    }

    void mapAreaSet()
    {
        float mapWidth = mapArea.GetComponent<RectTransform>().rect.width;
        float mapHeight = mapArea.GetComponent<RectTransform>().rect.height;

        float cameraHeight = 2 * Camera.main.orthographicSize;
        float cameraWidth = cameraHeight * Camera.main.aspect;

        Vector2 mapPotision = mapArea.transform.localPosition;

        float distanceWidth = Mathf.Abs(mapWidth / 2 - cameraWidth / 2);
        float distanceHeight = Mathf.Abs(mapHeight / 2 - cameraHeight / 2);

        min = new Vector2(mapPotision.x - distanceWidth, mapPotision.y - distanceHeight);
        max = new Vector2(mapPotision.x + distanceWidth, mapPotision.y + distanceHeight);
    }

    void LateUpdate()
    {
        float blockX = Mathf.Clamp(target.position.x, min.x, max.x);
        float blockY = Mathf.Clamp(target.position.y, min.y, max.y);

        transform.position = Vector2.Lerp(transform.position, new Vector2(blockX, blockY), speed);
    }
}
