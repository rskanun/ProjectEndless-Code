using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Navigator : MonoBehaviour
{
    public MapGrid testGrid;
    public Vector2 testStart;
    public Vector2 testEnd;

    public List<GridNode> testPath;

    // 탐색에 사용되는 변수
    private int[] sx = { 1, 1, 0, -1, -1, -1, 0, 1 };
    private int[] sy = { 0, -1, -1, -1, 0, 1, 1, 1 };
    private int prevSelectedIdx = -1;

    [Button("Test", ButtonSizes.Large)]
    public void Test()
    {
        testPath = FindPath(testGrid, testStart, testEnd);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(testStart, 0.5f);

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(testEnd, 0.5f);

        if (testPath == null || testPath.Count <= 1)
        {
            return;
        }

        Gizmos.color = Color.green;
        foreach (var node in testPath)
        {
            Gizmos.DrawCube(node.pos, Vector3.one * 0.5f);
        }
    }

    /// <summary>
    /// A* 알고리즘을 이용해 가장 빠른 노드 경로 탐색
    /// </summary>
    public List<GridNode> FindPath(MapGrid grid, Vector2 start, Vector2 end)
    {
        // 시작지점과 끝지점 각각 가까운 노드 탐색
        var startNode = GetNearNode(grid, start);
        var endNode = GetNearNode(grid, end);

        // 해당 노드 기준으로 길찾기
        return FindPath(grid, startNode, endNode);
    }

    private GridNode GetNearNode(MapGrid grid, Vector2 pos)
    {
        // 현재 맵의 가장 처음 노드 좌표를 기준으로 탐색
        var pivot = grid.GetNode(0, 0).pos;

        // 가장 가까운 노드 위치 찾기
        int x = Mathf.FloorToInt((pos.x - pivot.x) / grid.NodeSize + 0.5f);
        int y = Mathf.FloorToInt((pos.y - pivot.y) / grid.NodeSize + 0.5f);

        return grid.GetNode(x, y);
    }

    /// <summary>
    /// A* 알고리즘을 이용해 가장 빠른 노드 경로 탐색
    /// </summary>
    public List<GridNode> FindPath(MapGrid grid, GridNode start, GridNode end)
    {
        if (start == null || end == null)
        {
            return null;
        }

        var openList = new HashSet<GridNode>() { start };
        var closeList = new HashSet<GridNode>();
        var pq = new PriorityQueue<GridNode>();

        pq.Enqueue(start);

        // 이전 선택 초기화
        prevSelectedIdx = -1;

        while (pq.Count > 0)
        {
            var node = pq.Dequeue();

            if (closeList.Contains(node)) continue;

            // 도착 확인
            if (node == end)
            {
                // 경로를 역추적하여 리턴
                return RestracePath(grid, start, end);
            }

            openList.Remove(node);
            closeList.Add(node);

            // 주변 탐색
            foreach (var nearNode in GetNearNodes(grid, node))
            {
                // 갈 수 없는 구역이거나 이미 갔던 경우 넘어가기
                if (!nearNode.isWalkable || closeList.Contains(nearNode))
                {
                    continue;
                }

                // 미탐색 노드거나 더 적은 코스트인 경우
                int newCost = node.gCost + GetDistance(node, nearNode);
                if (!openList.Contains(nearNode) || newCost < nearNode.gCost)
                {
                    // gCost = 이전 노드 gCost + 둘 사이 거리
                    nearNode.gCost = newCost;
                    nearNode.hCost = GetDistance(nearNode, end);
                    nearNode.prevPos = node.gridPos;

                    pq.Enqueue(nearNode);
                    openList.Add(nearNode);
                }
            }
        }

        // 경로를 찾을 수 없는 경우 
        return null;
    }

    private List<GridNode> RestracePath(MapGrid grid, GridNode start, GridNode end)
    {
        var path = new List<GridNode>() { end };

        // 도착까지 경로 역탐색
        var node = end;
        while (node != start)
        {
            node = grid.GetNode(node.prevPos.x, node.prevPos.y);
            path.Add(node);
        }

        // 끝 -> 시작에서 시작 -> 끝으로 뒤집기
        path.Reverse();

        // 결과 리턴
        return path;
    }

    private List<GridNode> GetNearNodes(MapGrid grid, GridNode node)
    {
        var nodes = new List<GridNode>();

        // 이전 선택 방향 우선 삽입
        if (0 <= prevSelectedIdx && prevSelectedIdx < 8)
        {
            int mx = node.gridPos.x + sx[prevSelectedIdx];
            int my = node.gridPos.y + sy[prevSelectedIdx];

            var near = grid.GetNode(mx, my);

            if (near != null)
                nodes.Add(near);
        }

        // 주변 8칸 노드 가져오기
        for (int i = 0; i < 8; i++)
        {
            // 이미 삽입된 노드인 경우 건너뛰기
            if (i == prevSelectedIdx) continue;

            int mx = node.gridPos.x + sx[i];
            int my = node.gridPos.y + sy[i];

            var near = grid.GetNode(mx, my);

            if (near != null)
                nodes.Add(near);
        }

        return nodes;
    }

    private int GetDistance(GridNode a, GridNode b)
    {
        int x = Math.Abs(a.gridPos.x - b.gridPos.x);
        int y = Math.Abs(a.gridPos.y - b.gridPos.y);
        int shortest = (x > y) ? y : x;

        // 대각선을 14로 설정해서 int값으로 계산
        return 14 * shortest + 10 * Math.Abs(x - y);
    }
}