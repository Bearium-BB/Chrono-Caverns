using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AStarPathfinding : MonoBehaviour
{
    SquareGrid grid;
    public List<AStarBaseNode> Pathfind(AStarBaseNode startNode, AStarBaseNode targetNode)
    {
        List<AStarBaseNode> toSearch = new List<AStarBaseNode>() { startNode };
        List<AStarBaseNode> processed = new List<AStarBaseNode>();

        return new List<AStarBaseNode>();
    }

    public void GenerateNode(Vector3 pos, AStarBaseNode targetNode)
    {
        AStarBaseNode aStarBaseNode = new AStarBaseNode(pos, targetNode);

        List<Vector3> neighbors = grid.GetCellNeighbors(pos);

        for (int i = 0; i < neighbors.Count; i++)
        {
            AStarBaseNode neighborsAStarBaseNode = new AStarBaseNode(pos, targetNode, aStarBaseNode);
            aStarBaseNode.Neighbors.Add(neighborsAStarBaseNode);
        }


    }
}
