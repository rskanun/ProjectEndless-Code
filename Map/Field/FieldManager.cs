using UnityEngine;
using UnityEngine.Tilemaps;

[ExecuteAlways]
[RequireComponent(typeof(Tilemap))]
public class FieldManager : MonoBehaviour
{
    [Header("필드 정보")]
    [SerializeField]
    private BattleFieldData battleField;
    private Tilemap tilemap;

    [Header("게임 데이터")]
    [SerializeField]
    private GameData gameData;

    private void OnValidate()
    {
        tilemap = GetComponent<Tilemap>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            gameData.FieldTilemap = tilemap;
        }
    }
}