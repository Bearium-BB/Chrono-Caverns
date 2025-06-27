using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareGrid : MonoBehaviour
{
    public int width = 10;
    public int height = 10;
    public float cellSize = 1f;

    public List<Vector3> cells = new List<Vector3>();

    void Start()
    {
        GenerateGrid();

        List<Vector3> neighbors = GetCellNeighbors(new Vector3(4, 0, 4));
        foreach (Vector3 n in neighbors)
        {
            Debug.Log($"Neighbor: {new Vector2(n.x, n.z)}");
        }
    }

    void GenerateGrid()
    {
        cells.Clear();
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                float xPos = x * cellSize;
                float zPos = z * cellSize;
                cells.Add(new Vector3(xPos, 0, zPos));
            }
        }
    }

    public List<Vector3> GetCellNeighbors(Vector3 cell)
    {
        int col = Mathf.RoundToInt(cell.x);
        int row = Mathf.RoundToInt(cell.z);

        // Eight directions: cardinal + diagonal
        Vector2Int[] directions = new Vector2Int[]
        {
        new Vector2Int(1, 0),   // right
        new Vector2Int(-1, 0),  // left
        new Vector2Int(0, 1),   // up
        new Vector2Int(0, -1),  // down
        new Vector2Int(1, 1),   // up-right diagonal
        new Vector2Int(-1, 1),  // up-left diagonal
        new Vector2Int(1, -1),  // down-right diagonal
        new Vector2Int(-1, -1), // down-left diagonal
        };

        var neighbors = new List<Vector3>();

        foreach (var dir in directions)
        {
            int neighborCol = col + dir.x;
            int neighborRow = row + dir.y;

            neighbors.Add(new Vector3(neighborCol, 0, neighborRow));
        }

        return neighbors;
    }
}
