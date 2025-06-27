using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

public class HexNodeAStar
{
    public HexNode node;

    public List<HexNodeAStar> neighbors = new List<HexNodeAStar>();
    // A* variables
    public float gCost;
    public float hCost;
    public float fCost => gCost + hCost;
    public HexNodeAStar parent;

    public HexNodeAStar(HexNode node)
    {
        this.node = node;
    }
}
