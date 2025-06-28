using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class HexPathfinder
{
    public static Dictionary<Vector2Int, HexNodeAStar> GenerateCashGrid(HexGrid grid,HexNodeAStar start, HexNodeAStar goal)
    {
        Dictionary<Vector2Int, HexNodeAStar> aStarNodes = new Dictionary<Vector2Int, HexNodeAStar>();
        aStarNodes.TryAdd(start.node.coord, start);
        aStarNodes.TryAdd(goal.node.coord, goal);
        foreach (var gridNode in grid.nodes)
        {
            if (start.node.coord != gridNode.coord  && goal.node.coord != gridNode.coord)
            {
                HexNode node = new HexNode(gridNode);
                HexNodeAStar aNode = new HexNodeAStar(node);
                aStarNodes.Add(gridNode.coord, aNode);
            }
        }

        foreach (var node in aStarNodes.Values)
        {
            foreach (var dir in directions)
            {
                Vector2Int neighborCoord = node.node.coord + dir;
                if (aStarNodes.ContainsKey(neighborCoord))
                {
                    node.neighbors.Add(aStarNodes[neighborCoord]);
                }
            }
        }
        return aStarNodes;
    }

    public static readonly Vector2Int[] directions = new Vector2Int[]
    {
        new Vector2Int(1, 0),
        new Vector2Int(1, -1),
        new Vector2Int(0, -1),
        new Vector2Int(-1, 0),
        new Vector2Int(-1, 1),
        new Vector2Int(0, 1)
    };

    static float Heuristic(Vector2Int a, Vector2Int b)
    {
        return (Mathf.Abs(a.x - b.x)
              + Mathf.Abs(a.x + a.y - b.x - b.y)
              + Mathf.Abs(a.y - b.y)) / 2f;
    }

    public static List<HexNodeAStar> FindPath(HexNodeAStar start, HexNodeAStar goal,HexGrid grid, bool canFly = false, bool canJump = true)
    {
        Stopwatch stopwatch = Stopwatch.StartNew();

        var openSet = new List<HexNodeAStar> { start };
        var closedSet = new HashSet<HexNodeAStar>();

        Dictionary<Vector2Int, HexNodeAStar> aStarNodes = new Dictionary<Vector2Int, HexNodeAStar>();
        aStarNodes.TryAdd(start.node.coord, start);
        aStarNodes.TryAdd(goal.node.coord,goal);

        aStarNodes = GenerateCashGrid(grid, start, goal);

        start.gCost = 0;
        start.hCost = Heuristic(start.node.coord, goal.node.coord);
        start.parent = null;

        while (openSet.Count > 0)
        {
            // Get node with lowest fCost
            HexNodeAStar current = openSet.OrderBy(n => n.fCost).First();

            if (current.node.coord == goal.node.coord)
            {
                stopwatch.Stop();
                UnityEngine.Debug.Log($"GeneratedAStarNodes took {stopwatch.ElapsedMilliseconds} ms");
                return RetracePath(start, goal);

            }

            openSet.Remove(current);
            closedSet.Add(current);

            if (current.neighbors != null)
            {
                current.neighbors.Clear();

            }

            foreach (var dir in directions)
            {
                Vector2Int neighborCoord = current.node.coord + dir;
                if (grid.ContainsPosition(neighborCoord))
                {
                    current.neighbors.Add(aStarNodes[neighborCoord]);

                }
            }

            foreach (HexNodeAStar neighbor in current.neighbors)
            {
                if (!IsTraversable(neighbor, current, canFly, canJump) || closedSet.Contains(neighbor))
                    continue;

                float tentativeGCost = current.gCost + 1; // You can add terrain cost here later

                if (!openSet.Contains(neighbor) || tentativeGCost < neighbor.gCost)
                {
                    neighbor.gCost = tentativeGCost;
                    neighbor.hCost = Heuristic(neighbor.node.coord, goal.node.coord);
                    neighbor.parent = current;

                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                }
            }
        }

        stopwatch.Stop();
        UnityEngine.Debug.Log($"GeneratedAStarNodes took {stopwatch.ElapsedMilliseconds} ms");

        // No path found
        return null;
    }

    private static List<HexNodeAStar> RetracePath(HexNodeAStar start, HexNodeAStar goal)
    {
        List<HexNodeAStar> path = new List<HexNodeAStar>();
        HexNodeAStar current = goal;


        while (current != start)
        {
            path.Add(current);
            current = current.parent;
        }


        path.Add(start);
        path.Reverse();
        return path;
    }

    private static bool IsTraversable(HexNodeAStar node, HexNodeAStar fromNode, bool canFly, bool canJump)
    {
        if (!node.node.isWalkable)
            return false;

        int heightDiff = Mathf.Abs(node.node.heightLevel - fromNode.node.heightLevel);

        if (node.node.isFlyOnly && !canFly)
            return false;

        if ((node.node.isCliff || fromNode.node.isCliff) && heightDiff <= 1)
        {
            if (canJump)
                return true;
            return false;
        }

        if (node.node.isRamp || fromNode.node.isRamp)
        {
            return heightDiff <= 1;
        }

        // Normal height difference block (optional):
        if (Mathf.Abs(node.node.heightLevel - fromNode.node.heightLevel) > 0)
            return false;

        return true;
    }

}