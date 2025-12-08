using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class GridNode : IComparable<GridNode>
{
    // 항상 고정되는 값
    public bool isWalkable;
    public Vector2 pos;
    public Vector2Int gridPos;

    // 인게임 내에서 바뀌는 값
    [NonSerialized] public int gCost;
    [NonSerialized] public int hCost;
    public int fCost => gCost + hCost;

    public int CompareTo(GridNode other)
    {
        var compare = fCost.CompareTo(other.fCost);

        // fCost가 같은 경우 직선 거리(hCost)가 더 가까운 값 리턴
        if (compare == 0)
            return hCost.CompareTo(other.hCost);

        return compare;
    }
}

public class MapGrid : SerializedMonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private LayerMask obstacle;
    [SerializeField, MinValue(0.5f)] private float nodeSize = 1.0f;
    [SerializeField] private Vector2 offset;
    [SerializeField, Range(0.1f, 1.0f)] private float collisionScale = 1.0f;

    [SerializeField, HideInInspector]
    private GridNode[,] mapNodes;

    [Button("Reload Grid", ButtonSizes.Large)]
    public void ReloadGrid()
    {
        // 타일맵이 없는 경우 적용 X
        if (tilemap == null) return;

        // 맵 크기
        float width = tilemap.cellBounds.size.x;
        float height = tilemap.cellBounds.size.y;

        // 노드 배치 사이즈
        int horizonCount = Mathf.CeilToInt((width - offset.x * 2) / nodeSize);
        int verticalCount = Mathf.CeilToInt((height - offset.y * 2) / nodeSize);

        // 그리드 영역에 포함되지 못한 여백 공간
        // 그리드 영역은 가운데를 기준
        float gridOffsetX = (width - horizonCount * nodeSize) / 2.0f;
        float gridOffsetY = (height - verticalCount * nodeSize) / 2.0f;

        // 노드를 생성할 첫 좌표
        // 타일맵 시작 좌표 + 여백 + 중심
        float pivotX = tilemap.cellBounds.xMin + gridOffsetX + nodeSize / 2.0f;
        float pivotY = tilemap.cellBounds.yMin + gridOffsetY + nodeSize / 2.0f;
        var pivot = new Vector2(pivotX, pivotY);

        // 검사 사이즈(100%로 할 경우 경계선도 포함되기 때문에 미세하게 줄여서 체크)
        var checkSize = Vector2.one * nodeSize * collisionScale;

        // 각 노드에 정보 넣기
        mapNodes = new GridNode[verticalCount, horizonCount];
        for (int y = 0; y < verticalCount; y++)
        {
            for (int x = 0; x < horizonCount; x++)
            {
                // 노드 중심 좌표
                float posX = pivot.x + x * nodeSize;
                float posY = pivot.y + y * nodeSize;
                var pos = new Vector2(posX, posY);

                // 영역 내 장애물 유무
                var hitCollider = Physics2D.OverlapBox(pos, checkSize, 0f, obstacle);

                var node = new GridNode();
                node.pos = pos;
                node.gridPos = new Vector2Int(x, y);
                node.isWalkable = (hitCollider == null);

                mapNodes[y, x] = node;
            }
        }
    }

    public GridNode GetNode(int x, int y)
    {
        // 범위에서 벗어난 값인 경우 빈 값 리턴
        if ((0 > x || x >= mapNodes.GetLength(1)) || 0 > y || y >= mapNodes.GetLength(0))
        {
            return null;
        }

        return mapNodes[y, x];
    }

    public void OnDrawGizmosSelected()
    {
        if (mapNodes == null) return;

        foreach (var node in mapNodes)
        {
            Gizmos.color = (node.isWalkable) ? Color.white : Color.red;
            Gizmos.DrawCube(node.pos, Vector3.one * nodeSize / 2);
        }
    }
}