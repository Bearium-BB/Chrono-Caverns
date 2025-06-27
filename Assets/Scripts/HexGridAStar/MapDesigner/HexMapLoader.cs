using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class HexMapLoader : MonoBehaviour
{
    public GameObject hexPrefab;

    string json;
    HexGrid grid;
    Dictionary<Vector2Int, GameObject> hexObjects = new Dictionary<Vector2Int, GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        json = File.ReadAllText(@"C:\Users\brett\AppData\LocalLow\DefaultCompany\Chrono Cavernous\map.json");
        HexGrid nodes = JsonConvert.DeserializeObject<HexGrid>(json);

        grid = nodes;

        foreach (var node in grid.nodes.Values)
        {
            Vector3 pos = HexToWorld(node.hexCoord);
            GameObject hexGO = Instantiate(hexPrefab, new(pos.x, node.heightLevel, pos.z), Quaternion.Euler(90, 0, 0), transform);
            HexNodeDesigner hexNodeDesigner = hexGO.AddComponent<HexNodeDesigner>();
            hexNodeDesigner.HexNode = node;
            hexObjects[node.hexCoord] = hexGO;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    Vector3 HexToWorld(Vector2Int hex)
    {
        float x = Mathf.Sqrt(3f) * (hex.x + hex.y / 2f);
        float z = 1.5f * hex.y;
        return new Vector3(x, 0, z);
    }
}
