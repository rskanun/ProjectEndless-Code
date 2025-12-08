using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Navigator : MonoBehaviour
{
    /// <summary>
    /// A* 알고리즘을 이용해 가장 빠른 노드 경로 탐색
    /// </summary>
    public List<GridNode> FindPath(MapGrid grid, GridNode start, GridNode end)
    {
        var path = new List<GridNode>() { start };
        var pq = new PriorityQueue<GridNode>();

        pq.Enqueue(start);

        while (path.Last() != end && pq.Count > 0)
        {
            var node = pq.Dequeue();

            // 주변 탐색
            var nearNodes = GetNearNodes(grid, node);

            // 주변에 탐색할 노드가 없는 경우
            if (nearNodes.Count == 0)
            {
                // 다음 코스트가 높은 값 택하기
                continue;
            }


        }

        return path;
    }

    private List<GridNode> GetNearNodes(MapGrid grid, GridNode node)
    {
        int[] sx = { 1, 1, 0, -1, -1, -1, 0, 1 };
        int[] sy = { 0, -1, -1, -1, 0, 1, 1, 1 };

        // 주변 8칸 노드 가져오기
        var nodes = new List<GridNode>();
        for (int i = 0; i < 8; i++)
        {
            int mx = node.gridPos.x + sx[i];
            int my = node.gridPos.y + sy[i];

            var near = grid.GetNode(mx, my);

            if (near != null)
                nodes.Add(near);
        }

        return nodes;
    }
}