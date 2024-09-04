using UnityEngine;

public class DottedArrowLine : MonoBehaviour
{
    public GameObject endArrow;
    public float height = 1.0f;

    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    public void DrawLine(Vector2 start, Vector2 end)
    {
        int count = lineRenderer.positionCount;
        int segmentCount = lineRenderer.positionCount - 1;

        for (int i = 0; i < segmentCount; i++)
        {
            // 보간
            float t = (float)i / (count - 1);
            Vector2 point = GetParabolaPoint(start, end, height, t);

            lineRenderer.SetPosition(i, point);
        }

        for (int i = segmentCount; i < count; i++)
        {
            lineRenderer.SetPosition(i, lineRenderer.GetPosition(segmentCount - 1));
        }

        // 화살표 배치
        SetEndArrow(end);
    }

    private Vector2 GetParabolaPoint(Vector2 start, Vector2 end, float h, float t)
    {
        // 수평 방향으로 보간
        Vector2 midPoint = Vector2.Lerp(start, end, t);

        // 포물선 형태로 높이 설정
        float parabolaH = 4 * h * t * (1 - t);

        return new Vector2(midPoint.x, midPoint.y + parabolaH);
    }

    private void SetEndArrow(Vector2 endPoint)
    {
        endArrow.transform.position = endPoint;

        // 화살표 회전
        Vector2 direction = endPoint - (Vector2)lineRenderer.GetPosition(lineRenderer.positionCount - 2);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90.0f;

        endArrow.transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}