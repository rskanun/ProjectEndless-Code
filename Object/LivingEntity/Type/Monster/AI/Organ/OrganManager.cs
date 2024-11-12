using System.Collections.Generic;
using UnityEngine;

public class OrganManager : MonoBehaviour
{
    // 해당 몬스터의 탐지 기관 목록
    private List<DetectionOrgan> organs;

    private void Awake()
    {
        organs = new List<DetectionOrgan>();

        // Init organs in components
        DetectionOrgan[] findOrgans = gameObject.GetComponents<DetectionOrgan>();
        organs.AddRange(findOrgans);
    }

    public Vector3 DetectPlayer()
    {
        foreach (DetectionOrgan organ in organs)
        {
            Vector3 vec = organ.DetectPlayer();
            if ((Vector2)vec == ReadOnlyGameData.Instance.Position)
            {
                return vec;
            }
        }

        return transform.position;
    }

    public void RotateOrgans(Vector2 vec)
    {
        float angle = Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}