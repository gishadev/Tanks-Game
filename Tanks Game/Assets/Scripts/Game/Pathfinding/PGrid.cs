using System.Collections.Generic;
using UnityEngine;

public class PGrid : MonoBehaviour
{
    public Transform min, max;
    public LayerMask nonWalkableLayer;
    public float nodeRadius = 0.5f;

    int gridSizeX, gridSizeY;
    float nodeDiameter;

    Node[,] grid;

    void Awake()
    {
        nodeDiameter = nodeRadius * 2f;
        gridSizeX = Mathf.RoundToInt(Mathf.Abs(min.position.x) + Mathf.Abs(max.position.x));
        gridSizeY = Mathf.RoundToInt(Mathf.Abs(min.position.y) + Mathf.Abs(max.position.y));

        CreateGrid();
    }

    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];

        for (int x = 0; x < gridSizeX; x++)
            for (int y = 0; y < gridSizeY; y++)
            {
                float xPos = x * nodeDiameter + nodeRadius;
                float yPos = y * nodeDiameter + nodeRadius;
                Vector2 position = new Vector2(min.position.x + xPos, min.position.y + yPos);

                bool isWalkable = !Physics2D.OverlapCircle(position, nodeRadius / 2f, nonWalkableLayer);

                grid[x, y] = new Node(isWalkable, position, x, y);
            }
    }

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> result = new List<Node>();

        // Checking 3x3 around node. 
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                // Skipping because at (0;0) there is inputed node.
                if (x == 0 && y == 0)
                    continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                // Checking if current coords is in grid bounds.
                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                    result.Add(grid[checkX, checkY]);
            }
        }

        return result;
    }

    public Node GetNodeFromVector2(Vector2 point)
    {
        float percentX = (point.x / gridSizeX / 2f) / gridSizeX;
        float percentY = (point.y / gridSizeY / 2f) / gridSizeY;

        percentX = Mathf.Clamp01(percentX);
        percentX = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

        return grid[x, y];
    }

    void OnDrawGizmos()
    {
        if (grid != null)
            for (int x = 0; x < gridSizeX; x++)
                for (int y = 0; y < gridSizeY; y++)
                {
                    Gizmos.color = grid[x, y].isWalkable ? Color.white : Color.red;

                    if (Pathfinding.Instance.currentPath.Contains(grid[x, y]))
                        Gizmos.color = Color.black;

                    Gizmos.DrawWireCube(grid[x, y].worldPosition, Vector2.one * 1.9f * nodeRadius);
                }
    }

}
