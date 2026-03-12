using System.Collections.Generic;
using UnityEngine;

public class OrganManager : MonoBehaviour
{
    // 해당 몬스터의 탐지 기관 목록
    [ReadOnly, SerializeField]
    private List<DetectionOrgan> organs;

#if UNITY_EDITOR
    [ContextMenu("Reload Organs")]
    private void OnValidate()
    {
        organs.Clear();

        DetectionOrgan[] finds = gameObject.GetComponents<DetectionOrgan>();
        organs.AddRange(finds);
    }
#endif

    public Vector3? DetectPlayer()
    {
        // 모든 신체기관에서 플레이어 탐지
        foreach (DetectionOrgan organ in organs)
        {
            // 플레이어 위치 탐색에 성공했다면 좌표 보내기
            if (organ.DetectPlayer() is Vector3 vec)
            {
                return vec;
            }
        }

        // 찾지 못했다면 빈 값 리턴
        return null;
    }

    public void RotateOrgans(Vector2 vec)
    {
        float angle = Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}