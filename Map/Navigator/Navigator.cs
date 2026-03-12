using System;
using System.Collections.Generic;
using UnityEngine;

public static class Navigator
{
    /// <summary>
    /// A* 알고리즘을 이용해 가장 빠른 노드 경로 탐색
    /// </summary>
    public static List<Vector2> FindPath(MapGrid grid, Vector2 start, Vector2 end)
    {
        // 시작지점과 끝지점 각각 가까운 노드 탐색
        var startNode = GetNearNode(grid, start);
        var endNode = GetNearNode(grid, end);

        // 해당 노드 기준으로 길찾기
        return FindPath(grid, startNode, endNode);
    }

    private static GridNode GetNearNode(MapGrid grid, Vector2 pos)
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
    public static List<Vector2> FindPath(MapGrid grid, GridNode start, GridNode end)
    {
        if (start == null || end == null)
        {
            return null;
        }

        var openList = new HashSet<GridNode>() { start };
        var closeList = new HashSet<GridNode>();
        var pq = new PriorityQueue<GridNode>();

        pq.Enqueue(start);

        start.prevPos = start.gridPos;
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

            // 현재 노드의 진행 방향
            var dir = node.gridPos - node.prevPos;

            // 주변 탐색
            foreach (var nearNode in GetNearNodes(grid, node))
            {
                // 갈 수 없는 구역이거나 이미 갔던 경우 넘어가기
                if (!nearNode.isWalkable || closeList.Contains(nearNode))
                {
                    continue;
                }

                // 진행 방향이 동일한지 판단
                var nearDir = nearNode.gridPos - node.gridPos;
                bool isTurned = (dir - nearDir) != Vector2.zero;

                // gCost = 이전 노드 gCost + 둘 사이 거리
                int dirCost = isTurned ? 5 : 0;
                int newCost = node.gCost + GetDistance(node, nearNode) + dirCost;

                // 미탐색 노드거나 더 적은 코스트인 경우
                if (!openList.Contains(nearNode) || newCost < nearNode.gCost)
                {
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

    private static List<Vector2> RestracePath(MapGrid grid, GridNode start, GridNode end)
    {
        var path = new List<Vector2>() { end.pos };

        // 도착까지 경로 역탐색
        var node = end;
        while (node != start)
        {
            node = grid.GetNode(node.prevPos.x, node.prevPos.y);
            path.Add(node.pos);
        }

        // 끝 -> 시작에서 시작 -> 끝으로 뒤집기
        path.Reverse();

        // 결과 리턴
        return path;
    }

    private static List<GridNode> GetNearNodes(MapGrid grid, GridNode node)
    {
        int[] sx = { 1, 1, 0, -1, -1, -1, 0, 1 };
        int[] sy = { 0, -1, -1, -1, 0, 1, 1, 1 };

        var nodes = new List<GridNode>();

        // 주변 8칸 노드 가져오기
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

    private static int GetDistance(GridNode a, GridNode b)
    {
        int x = Math.Abs(a.gridPos.x - b.gridPos.x);
        int y = Math.Abs(a.gridPos.y - b.gridPos.y);
        int shortest = (x > y) ? y : x;

        // 빠른 계산을 위해 대각선은 14, 나머지 진선 거리를 10으로 계산
        return 14 * shortest + 10 * Math.Abs(x - y);
    }
}