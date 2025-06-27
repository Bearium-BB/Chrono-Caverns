using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

public class HexNode
{
    public Vector2Int hexCoord;
    [JsonIgnore]
    public List<HexNode> neighbors = new List<HexNode>();

    // A* variables
    public float gCost;
    public float hCost;
    public float fCost => gCost + hCost;
    [JsonIgnore]
    public HexNode parent;

    public bool isWalkable = true;

    // Terrain types
    public bool isRamp;
    public bool isCliff;
    public bool isFlyOnly;
    public int heightLevel;

    public bool isJumpable => isCliff && heightLevel <= 1;

    public HexNode(Vector2Int coord)
    {
        hexCoord = coord;
        isWalkable = true;
    }
}
