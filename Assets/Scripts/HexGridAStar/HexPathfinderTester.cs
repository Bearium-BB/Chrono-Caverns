using System.CodeDom.Compiler;
using System.Collections.Generic;
using UnityEngine;

public class HexPathfinderTester : MonoBehaviour
{
    public int gridRadius = 5;
    public GameObject hexPrefab;

    HexGrid grid;
    Dictionary<Vector2Int, GameObject> hexObjects = new Dictionary<Vector2Int, GameObject>();

    List<HexNodeAStar> path;

    [Range(0f, 1f)]
    public float obstacleChance = 0.2f;

    void Update()
    {
        Vector3 worldPosition = Vector3.zero;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            worldPosition = hitInfo.point;
            //Debug.Log("Mouse hit world position: " + worldPosition);
        }

        HexNode startNode = new HexNode(new Vector2Int(0, 0));
        HexNodeAStar aStarNode = new HexNodeAStar(startNode);

        HexNode goalNode = new HexNode(WorldToHex(worldPosition));
        HexNodeAStar aGoalNode = new HexNodeAStar(goalNode);

        path = HexPathfinder.FindPath(aStarNode, aGoalNode, grid, canFly: false, canJump: false);
        HighlightPath();
    }

    void Start()
    {
        grid = new HexGrid(gridRadius);

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


        // Instantiate hex tiles and set colors
        foreach (var node in grid.nodes)
        {
            Vector3 pos = HexToWorld(node.coord);
            var hexGO = Instantiate(hexPrefab, new(pos.x, 0, pos.z), Quaternion.Euler(90, 0, 0), transform);

            // Start building the name
            string name = $"Hex_{node.coord.x}_{node.coord.y}";

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

            hexObjects[node.coord] = hexGO;
        }
    }

    public HexNodeAStar GetAHexByPosition(Vector2Int pos)
    {
        if(path == null)
            return null;    
        foreach (HexNodeAStar node in path)
        {
            if (node.node.coord == pos)
            {
                return node;
            }
        }
        return null;
    }


    void HighlightPath()
    {
        if (path == null) return;

        foreach (var kvp in hexObjects)
        {
            GameObject hexGO = kvp.Value;
            Renderer renderer = hexGO.GetComponent<Renderer>();
            HexNodeAStar node = GetAHexByPosition(path[0].node.coord);


            if (!node.node.isWalkable)
            {
                renderer.material.color = Color.red;
            }
            else
            {
                renderer.material.color = Color.white;
            }
        }

        if (path == null) return;

        foreach (var node in path)
        {

            if (!node.node.isWalkable) continue; // skip obstacles

            var renderer = hexObjects[node.node.coord].GetComponent<Renderer>();
            renderer.material.color = Color.green;
        }
    }


    Vector3 HexToWorld(Vector2Int hex)
    {
        float x = Mathf.Sqrt(3f) * (hex.x + hex.y / 2f);
        float z = 1.5f * hex.y;
        return new Vector3(x, 0, z);
    }

    Vector2Int WorldToHex(Vector3 position)
    {
        float q = (Mathf.Sqrt(3f) / 3f * position.x - 1f / 3f * position.z) / 1f;
        float r = (2f / 3f * position.z) / 1f;

        // Round to nearest hex (axial rounding)
        return HexRound(q, r);
    }

    Vector2Int HexRound(float q, float r)
    {
        float s = -q - r;
        int rq = Mathf.RoundToInt(q);
        int rr = Mathf.RoundToInt(r);
        int rs = Mathf.RoundToInt(s);

        float dq = Mathf.Abs(rq - q);
        float dr = Mathf.Abs(rr - r);
        float ds = Mathf.Abs(rs - s);

        if (dq > dr && dq > ds)
        {
            rq = -rr - rs;
        }
        else if (dr > ds)
        {
            rr = -rq - rs;
        }

        return new Vector2Int(rq, rr);
    }


}
