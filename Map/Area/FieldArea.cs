using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
public class FieldArea : MonoBehaviour
{
    // 마지막 방문 구역
    private static FieldArea lastEntedArea;
    private static FieldArea lastExitedArea;

    [Header("구역 정보")]
    [SerializeField]
    private PolygonCollider2D areaCollider;
    [SerializeField]
    private List<GameObject> fieldMonsters;
    [SerializeField]
    private BattleFieldData fieldData;

#if UNITY_EDITOR
    private void OnValidate()
    {
        areaCollider = GetComponent<PolygonCollider2D>();
    }
#endif

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 첫 방문 구역일 경우
            if (lastEntedArea == null)
            {
                // 해당 구역 활성화
                lastExitedArea = this;
                EnableArea(this);
            }

            // 마지막 방문 구역으로 등록
            lastEntedArea = this;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 현재 있는 구역에서 나가게 되면 
            if (lastEntedArea == areaCollider)
            {
                // 나간 영역이 마지막 방문 구역일 경우 무시
                return;
            }

            // 이전 구역으로 등록
            lastExitedArea = this;

            // 마지막으로 방문한 구역을 카메라 영역으로 변경
            EnableArea(lastEntedArea);
        }
    }

    private void EnableArea(FieldArea area)
    {
        // 해당 구역을 카메라 영역으로 지정
        MapManager.SetCurrentArea(area.areaCollider);

        // 구역 몬스터 활성화
        foreach (GameObject mobObj in area.fieldMonsters)
        {
            if (mobObj != null) mobObj.SetActive(true);
        }

        // 이전 구역 비활성화
        if (lastExitedArea != null && lastExitedArea != area)
        {
            DisableArea(lastExitedArea);
        }
    }

    private void DisableArea(FieldArea area)
    {
        // 구역 몬스터 비활성화
        foreach (GameObject mobObj in area.fieldMonsters)
        {
            if (mobObj != null) mobObj.SetActive(false);
        }
    }
}