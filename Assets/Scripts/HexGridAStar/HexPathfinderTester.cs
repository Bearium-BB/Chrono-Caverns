using System.CodeDom.Compiler;
using System.Collections.Generic;
using UnityEngine;

public class HexPathfinderTester : MonoBehaviour
{
    public int gridRadius = 5;
    public GameObject hexPrefab;
    public Material pathMaterial;
    public Material defaultMaterial;

    HexGrid grid;
    Dictionary<Vector2Int, GameObject> hexObjects = new Dictionary<Vector2Int, GameObject>();
    Dictionary<Vector2Int, HexNode> nodes = new Dictionary<Vector2Int, HexNode>();

    HexNode startNode;
    HexNode goalNode;
    List<HexNode> path;

    [Range(0f, 1f)]
    public float obstacleChance = 0.2f;

    static readonly Vector2Int[] directions = new Vector2Int[]
    {
        new Vector2Int(1, 0),
        new Vector2Int(1, -1),
        new Vector2Int(0, -1),
        new Vector2Int(-1, 0),
        new Vector2Int(-1, 1),
        new Vector2Int(0, 1)
    };


    void Start()
    {

        grid = new HexGrid(gridRadius);
        GeneratedNeighbors(gridRadius);

        // Randomly assign obstacles
        //System.Random rng = new System.Random();
        //foreach (var node in nodes.Values)
        //{
        //    node.heightLevel = rng.Next(0, 4);

        //    // 10% cliff, 10% ramp, 5% fly-only
        //    node.isCliff = rng.NextDouble() < 0.1;
        //    node.isRamp = rng.NextDouble() < 0.1;
        //    node.isFlyOnly = rng.NextDouble() < 0.05;

        //    if (rng.NextDouble() < obstacleChance)
        //        node.isWalkable = false;
        //}


        // Make sure start and goal are always walkable
        startNode = nodes[new Vector2Int(5,5)];
        goalNode = nodes[new Vector2Int(gridRadius - 1, -gridRadius + 1)];

        startNode.isWalkable = true;
        goalNode.isWalkable = true;

        // Instantiate hex tiles and set colors
        foreach (var node in grid.nodes)
        {
            Vector3 pos = HexToWorld(node);
            var hexGO = Instantiate(hexPrefab, new(pos.x, 0, pos.z), Quaternion.Euler(90, 0, 0), transform);

            // Start building the name
            string name = $"Hex_{node.x}_{node.y}";

            // Add descriptive tags to the name
            //if (!node.isWalkable) name += "_Blocked";
            //if (node.isRamp) name += "_Ramp";
            //if (node.isCliff) name += "_Cliff";
            //if (node.isFlyOnly) name += "_FlyOnly";
            //if (node.isJumpable) name += "_Jumpable";
            //name += $"_H{node.heightLevel}";

            hexGO.name = name;

            // Set color or material
            //Renderer renderer = hexGO.GetComponent<Renderer>();
            //if (!node.isWalkable)
            //    renderer.material.color = Color.red;
            //else
            //    renderer.material = defaultMaterial;

            hexObjects[node] = hexGO;
        }

        // Find path
        path = HexPathfinder.FindPath(startNode, goalNode, canFly: false, canJump: false);

        HighlightPath();
    }

    void HighlightPath()
    {
        foreach (var kvp in hexObjects)
        {
            var hexGO = kvp.Value;
            var renderer = hexGO.GetComponent<Renderer>();
            var node = nodes[kvp.Key];

            if (!node.isWalkable)
            {
                renderer.material.color = Color.red;
            }
            else
            {
                renderer.material = defaultMaterial;
            }
        }

        if (path == null) return;

        foreach (var node in path)
        {

            if (!node.isWalkable) continue; // skip obstacles

            var renderer = hexObjects[node.hexCoord].GetComponent<Renderer>();
            renderer.material = pathMaterial;
        }
    }


    // Convert axial hex coordinates to world position (pointy topped)
    Vector3 HexToWorld(Vector2Int hex)
    {
        float x = Mathf.Sqrt(3f) * (hex.x + hex.y / 2f);
        float z = 1.5f * hex.y;
        return new Vector3(x, 0, z);
    }

    public void GeneratedNeighbors(int radius)
    {
        for (int q = -radius; q <= radius; q++)
        {
            int r1 = Mathf.Max(-radius, -q - radius);
            int r2 = Mathf.Min(radius, -q + radius);
            for (int r = r1; r <= r2; r++)
            {
                var node = new HexNode(new Vector2Int(q, r));
                nodes.Add(new Vector2Int(q, r), node);
            }
        }

        foreach (var node in nodes.Values)
        {
            foreach (var dir in directions)
            {
                Vector2Int neighborCoord = node.hexCoord + dir;
                if (nodes.ContainsKey(neighborCoord))
                {
                    node.neighbors.Add(nodes[neighborCoord]);
                }
            }
        }
    }
}
