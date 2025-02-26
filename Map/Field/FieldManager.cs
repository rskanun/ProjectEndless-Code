using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(TilemapCollider2D))]
public class FieldManager : MonoBehaviour
{
    [Header("필드 정보")]
    [SerializeField]
    private BattleFieldData battleField;
    [SerializeField]
    private TilemapCollider2D cameraArea;

#if UNITY_EDITOR
    private void OnValidate()
    {
        cameraArea = GetComponent<TilemapCollider2D>();
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