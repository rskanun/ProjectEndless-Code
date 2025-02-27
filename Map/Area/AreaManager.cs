using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(PolygonCollider2D))]
public class AreaManager : MonoBehaviour
{
    [Header("구역 정보")]
    [SerializeField]
    private BattleFieldData battleField;
    [SerializeField]
    private PolygonCollider2D cameraArea;

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
            MapManager.SetCurrentArea(cameraArea);
        }
    }
}