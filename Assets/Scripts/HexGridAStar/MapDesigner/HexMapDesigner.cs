using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class HexMapDesigner : MonoBehaviour
{
    HexGrid grid;
    public GameObject hexPrefab;

    HexNode startNode;
    HexNode goalNode;
    List<HexNode> path;

    Dictionary<Vector2Int, GameObject> hexObjects = new Dictionary<Vector2Int, GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        grid = new HexGrid(20);
        foreach (var node in grid.nodes.Values)
        {
            Vector3 pos = HexToWorld(node.hexCoord);
            GameObject hexGO = Instantiate(hexPrefab, new(pos.x, node.heightLevel, pos.z), Quaternion.Euler(90, 0, 0), transform);
            HexNodeDesigner hexNodeDesigner = hexGO.AddComponent<HexNodeDesigner>();
            hexNodeDesigner.HexNode = node;
            hexObjects[node.hexCoord] = hexGO;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            startNode = grid.nodes[new Vector2Int(0, 0)];
            goalNode = grid.nodes[new Vector2Int(20 - 1, -20 + 1)];
            path = HexPathfinder.FindPath(startNode, goalNode, canFly: false, canJump: false);
            HighlightPath();

            string json = JsonConvert.SerializeObject(grid, Formatting.Indented);
            string pathJson = Path.Combine(Application.persistentDataPath, "map.json");
            Debug.Log(pathJson);
            File.WriteAllText(pathJson, json);
        }
    }

    void HighlightPath()
    {
        foreach (var kvp in hexObjects)
        {
            var hexGO = kvp.Value;
            var renderer = hexGO.GetComponent<Renderer>();

            renderer.material.color = Color.white;
        }


        foreach (var kvp in hexObjects)
        {
            var hexGO = kvp.Value;
            var renderer = hexGO.GetComponent<Renderer>();
            var node = grid.nodes[kvp.Key];

            if (!node.isWalkable)
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
            if (!node.isWalkable) continue; // skip obstacles

            var renderer = hexObjects[node.hexCoord].GetComponent<Renderer>();
            renderer.material.color = Color.green;
        }
    }

    Vector3 HexToWorld(Vector2Int hex)
    {
        float x = Mathf.Sqrt(3f) * (hex.x + hex.y / 2f);
        float z = 1.5f * hex.y;
        return new Vector3(x, 0, z);
    }
}
