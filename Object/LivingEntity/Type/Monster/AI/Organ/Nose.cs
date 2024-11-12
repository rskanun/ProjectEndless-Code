using UnityEngine;

public class Nose : DetectionOrgan
{
    [Header("탐지 반경")]
    [SerializeField] protected float detectRadius;

    public override Vector3 DetectPlayer()
    {
        ReadOnlyGameData playerData = ReadOnlyGameData.Instance;

        // 일정 반경 안에 있는 플레이어 무조건 탐색
        Vector2 playerVec = playerData.Position - (Vector2)transform.position;
        float distance = playerVec.magnitude;

        if (distance <= detectRadius)
        {
            // 범위 안에 있는 플레이어 위치 리턴
            return playerData.Position;
        }

        // 범위 안에 플레이어가 없으면 본인 위치 리턴
        return transform.position;
    }

    public void OnDrawGizmos()
    {
        // 탐지 반경
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, detectRadius);
    }
}