using UnityEngine;

public class Eye : DetectionOrgan
{
    [Header("시야각")]
    [SerializeField, Range(0, 360)]
    protected float viewAngle;

    [Header("탐지 거리")]
    [SerializeField] protected float detectDistance;

    public override Vector3? DetectPlayer()
    {
        GameData playerData = GameData.Instance;

        // 플레이어와 거리 계산
        Vector2 playerVec = playerData.Position - (Vector2)transform.position;
        float distance = playerVec.magnitude;

        // 플레이어가 지정된 범위 안에 있는지 확인
        if (distance <= detectDistance && (viewAngle >= 360 || PlayerInAngle(playerVec.normalized)))
        {
            // 범위 안에 있는 플레이어 위치 리턴
            return playerData.Position;
        }

        return null;
    }

    private bool PlayerInAngle(Vector2 playerVec)
    {
        // 시야각 범위 계산
        float halfViewAngle = viewAngle / 2f;
        float startAngle = transform.eulerAngles.z - halfViewAngle;
        float endAngle = transform.eulerAngles.z + halfViewAngle;

        endAngle = (endAngle > 180) ? endAngle - 360 : endAngle;

        // 플레이어와의 각도 계산
        float angle = Mathf.Atan2(playerVec.y, playerVec.x) * Mathf.Rad2Deg;

        // 시작 각도와 끝 각도 사이에 플레이어가 있는지 판별
        if (startAngle <= endAngle)
        {
            return startAngle <= angle && angle <= endAngle;
        }

        return startAngle <= angle || angle <= endAngle;
    }

    public void OnDrawGizmos()
    {
        // 각도를 라디안으로 변환
        float angleRadians = transform.eulerAngles.z * Mathf.Deg2Rad;

        // 각도 범위 계산
        float halfViewAngleRadians = viewAngle / 2f * Mathf.Deg2Rad;
        float startAngle = angleRadians - halfViewAngleRadians;
        float endAngle = angleRadians + halfViewAngleRadians;

        // 시작점과 끝점 계산
        Vector2 startPos = transform.position;
        Vector2 endPos1 = startPos + new Vector2(Mathf.Cos(startAngle), Mathf.Sin(startAngle)) * detectDistance;
        Vector2 endPos2 = startPos + new Vector2(Mathf.Cos(endAngle), Mathf.Sin(endAngle)) * detectDistance;
        Vector2 endPos3 = startPos + new Vector2(Mathf.Cos(angleRadians), Mathf.Sin(angleRadians)) * detectDistance;

        // 탐지 반경
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, detectDistance);

        // 시야각 그리기(360도가 넘어가면 무시)
        if (viewAngle < 360)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(startPos, endPos1);
            Gizmos.DrawLine(startPos, endPos2);
        }

        // 응시각 그리기
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(startPos, endPos3);
    }
}