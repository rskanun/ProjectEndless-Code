using UnityEngine;

public class Nose : DetectionOrgan
{
    [Header("탐지 반경")]
    [SerializeField] protected float detectRadius;

    public override Vector3? DetectPlayer()
    {
        ReadOnlyGameData playerData = ReadOnlyGameData.Instance;

        // 일정 반경 안에 있는 플레이어 무조건 탐색
        float distance = Vector2.Distance(playerData.Position, transform.position);
        if (distance <= detectRadius)
        {
            // 범위 안에 있는 플레이어 위치 리턴
            return playerData.Position;
        }

        return null;
    }

    public void OnDrawGizmos()
    {
        // 탐지 반경
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, detectRadius);
    }
}