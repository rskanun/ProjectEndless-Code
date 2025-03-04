using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(PolygonCollider2D))]
public class AreaManager : MonoBehaviour
{
    private static PolygonCollider2D lastEntedArea;
    [Header("구역 정보")]
    [SerializeField]
    private PolygonCollider2D cameraArea;
    [SerializeField]
    private BattleFieldData battleField;

#if UNITY_EDITOR
    private void OnValidate()
    {
        cameraArea = GetComponent<PolygonCollider2D>();
    }
#endif

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            OnEnterArea();
        }
    }

    private void OnEnterArea()
    {
        // 해당 구역이 처음이자 마지막인 경우
        if (lastEntedArea == null)
        {
            // 현재 있는 구역으로 지정
            MapManager.SetCurrentArea(cameraArea);
        }

        // 마지막 방문 구역으로 등록
        lastEntedArea = cameraArea;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            OnExitArea();
        }
    }

    private void OnExitArea()
    {
        // 현재 있는 구역에서 나가게 되면 
        if (lastEntedArea == cameraArea)
        {
            // 나간 영역이 마지막 방문 구역일 경우 되돌리기
            lastEntedArea = MapManager.CurrentArea;
            return;
        }

        // 마지막으로 방문한 구역을 카메라 영역으로 변경
        MapManager.SetCurrentArea(lastEntedArea);
    }
}