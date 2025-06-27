using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HexPathfinder
{
    // Heuristic: hex distance (Manhattan distance in axial coordinates)
    static float Heuristic(Vector2Int a, Vector2Int b)
    {
        return (Mathf.Abs(a.x - b.x)
              + Mathf.Abs(a.x + a.y - b.x - b.y)
              + Mathf.Abs(a.y - b.y)) / 2f;
    }

    public static List<HexNode> FindPath(HexNode start, HexNode goal, bool canFly = false, bool canJump = true)
    {
        var openSet = new List<HexNode> { start };
        var closedSet = new HashSet<HexNode>();

        start.gCost = 0;
        start.hCost = Heuristic(start.hexCoord, goal.hexCoord);
        start.parent = null;

        while (openSet.Count > 0)
        {
            // Get node with lowest fCost
            HexNode current = openSet.OrderBy(n => n.fCost).First();

            if (current == goal)
            {
                return RetracePath(start, goal);
            }

            openSet.Remove(current);
            closedSet.Add(current);

            foreach (var neighbor in current.neighbors)
            {
                Debug.Log("Start neighbor");

                if (!IsTraversable(neighbor, current, canFly, canJump) || closedSet.Contains(neighbor))
                    continue;

                float tentativeGCost = current.gCost + 1; // You can add terrain cost here later

                if (!openSet.Contains(neighbor) || tentativeGCost < neighbor.gCost)
                {
                    neighbor.gCost = tentativeGCost;
                    neighbor.hCost = Heuristic(neighbor.hexCoord, goal.hexCoord);
                    neighbor.parent = current;

                    if (!openSet.Contains(neighbor))
                        openSet.Add(neighbor);
                }
            }
            Debug.Log("End neighbor");
            Debug.Log(openSet.Count);


        }

        // No path found
        return null;
    }

    private static List<HexNode> RetracePath(HexNode start, HexNode goal)
    {
        List<HexNode> path = new List<HexNode>();
        HexNode current = goal;

        while (current != start)
        {
            path.Add(current);
            current = current.parent;
        }

        path.Add(start);
        path.Reverse();
        return path;
    }

    private static bool IsTraversable(HexNode node, HexNode fromNode, bool canFly, bool canJump)
    {
        if (!node.isWalkable)
            return false;

        int heightDiff = Mathf.Abs(node.heightLevel - fromNode.heightLevel);

        if (node.isFlyOnly && !canFly)
            return false;

        if ((node.isCliff || fromNode.isCliff) && heightDiff <= 1)
        {
            if (canJump)
                return true;
            return false;
        }

        if (node.isRamp || fromNode.isRamp)
        {
            return heightDiff <= 1;
        }

        // Normal height difference block (optional):
        if (Mathf.Abs(node.heightLevel - fromNode.heightLevel) > 0)
            return false;

        return true;
    }

}