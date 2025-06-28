using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HexNode
{
    public Vector2Int coord;

    public bool isWalkable;
    public bool isRamp;
    public bool isCliff;
    public bool isFlyOnly;
    public int heightLevel;
    public bool isJumpable => isCliff && heightLevel <= 1;

    public HexNode(Vector2Int coord, bool isWalkable = true, bool isRamp = false, bool isCliff = false, bool isFlyOnly = false, int heightLevel = 0)
    {
        this.coord = coord;
        this.isWalkable = isWalkable;
        this.isRamp = isRamp;
        this.isCliff = isCliff;
        this.isFlyOnly = isFlyOnly;
        this.heightLevel = heightLevel;
    }

    public HexNode(HexNode node)
    {
        this.coord = node.coord;
        this.isWalkable = node.isWalkable;
        this.isRamp = node.isRamp;
        this.isCliff = node.isCliff;
        this.isFlyOnly = node.isFlyOnly;
        this.heightLevel = node.heightLevel;
    }
}
