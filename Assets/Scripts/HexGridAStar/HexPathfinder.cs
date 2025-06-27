using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HexPathfinder
{
    static float Heuristic(Vector2Int a, Vector2Int b)
    {
        return (Mathf.Abs(a.x - b.x)
              + Mathf.Abs(a.x + a.y - b.x - b.y)
              + Mathf.Abs(a.y - b.y)) / 2f;
    }

    public static List<HexNodeAStar> FindPath(HexNodeAStar start, HexNodeAStar goal, bool canFly = false, bool canJump = true)
    {
        var openSet = new List<HexNodeAStar> { start };
        var closedSet = new HashSet<HexNodeAStar>();

        start.gCost = 0;
        start.hCost = Heuristic(start.node.coord, goal.node.coord);
        start.parent = null;

        while (openSet.Count > 0)
        {
            // Get node with lowest fCost
            HexNodeAStar current = openSet.OrderBy(n => n.fCost).First();

            if (current == goal)
            {
                return RetracePath(start, goal);
            }

            openSet.Remove(current);
            closedSet.Add(current);

            foreach (var neighbor in current.neighbors)
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
                        openSet.Add(neighbor);
                }
            }
        }

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