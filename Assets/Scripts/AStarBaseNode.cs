using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AStarBaseNode : MonoBehaviour
{
    public int G => Connection == null ? 0 : 1 + Connection.G;
    public float H;
    public float F => G + H;
    public Vector2 Coordinate;
    public List<AStarBaseNode> Neighbors;
    public AStarBaseNode Connection;

    public AStarBaseNode(Vector2 coordinate, AStarBaseNode targetNode, AStarBaseNode connection = null, List<AStarBaseNode> neighbors = null)
    {
        Coordinate = coordinate;
        Neighbors = neighbors ?? new List<AStarBaseNode>();
        H = Vector2.Distance(Coordinate, targetNode.Coordinate);
        Connection = connection;
    }

    public int CountConnections(AStarBaseNode node)
    {
        if (node == null || node.Connection == null)
            return 0;
        return 1 + CountConnections(node.Connection);
    }


}
