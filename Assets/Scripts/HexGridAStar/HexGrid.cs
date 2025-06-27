using System.Collections.Generic;
using UnityEngine;

public class HexGrid
{
    public List<HexNode> nodes = new List<HexNode>();

    public HexGrid(int radius)
    {
        // Generate hex grid in a radius from center (0,0)
        for (int q = -radius; q <= radius; q++)
        {
            int r1 = Mathf.Max(-radius, -q - radius);
            int r2 = Mathf.Min(radius, -q + radius);
            for (int r = r1; r <= r2; r++)
            {
                nodes.Add(new HexNode(new Vector2Int(q, r)));
            }
        }

        // Link neighbors
        //foreach (var node in nodes.Values)
        //{
        //    foreach (var dir in directions)
        //    {
        //        Vector2Int neighborCoord = node.hexCoord + dir;
        //        if (nodes.ContainsKey(neighborCoord))
        //        {
        //            node.neighbors.Add(nodes[neighborCoord]);
        //        }
        //    }
        //}
    }
}
