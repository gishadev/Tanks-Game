﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class PGrid : MonoBehaviour
{
    public Transform min, max;
    public LayerMask nonWalkableLayer;
    public float nodeRadius = 0.5f;

    [HideInInspector] public int gridSizeX;
    [HideInInspector] public int gridSizeY;
    float nodeDiameter;

    public Node[,] grid;
    public VisualGrid visual;

    void Awake()
    {
        nodeDiameter = nodeRadius * 2f;
        gridSizeX = Mathf.RoundToInt(Mathf.Abs(min.position.x) + Mathf.Abs(max.position.x));
        gridSizeY = Mathf.RoundToInt(Mathf.Abs(min.position.y) + Mathf.Abs(max.position.y));

        CreateGrid();
        visual.CreateGrid(this);
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

    public void UpdateGrid()
    {
        for (int x = 0; x < gridSizeX; x++)
            for (int y = 0; y < gridSizeY; y++)
            {
                bool isWalkable = !Physics2D.OverlapCircle(grid[x,y].worldPosition, nodeRadius / 2f, nonWalkableLayer);
                grid[x, y].isWalkable = isWalkable;
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
        int x = Mathf.CeilToInt(Mathf.CeilToInt(point.x + Mathf.Abs(min.position.x))) - 1;
        int y = Mathf.CeilToInt(Mathf.CeilToInt(point.y + Mathf.Abs(min.position.y))) - 1;

        return grid[x, y];
    }

    //public bool IsBlockedWithUnit(Node node)
    //{
    //    return UnitsManager.Instance.units.Any(x => x.Value.CurrentNode == node);
    //}

    void OnDrawGizmos()
    {
        if (grid != null)
            for (int x = 0; x < gridSizeX; x++)
                for (int y = 0; y < gridSizeY; y++)
                {
                    Gizmos.color = grid[x, y].isWalkable ? Color.white : Color.red;

                    // Paths of units.
                    foreach (UnitController unit in UnitsManager.Instance.units.Values)
                    {
                        if (unit.Movement.currentPath.Contains(grid[x, y]))
                            Gizmos.color = Color.black;
                    }

                    // Selected unit.
                    if (PhotonRoom.Instance.photonPlayers[0].SelectedUnit != null)
                    {
                        Node selectedUnitNode = PhotonRoom.Instance.photonPlayers[0].SelectedUnit.CurrentNode;
                        if (selectedUnitNode.gridX == x && selectedUnitNode.gridY == y)
                            Gizmos.color = Color.cyan;
                    }

                    Gizmos.DrawWireCube(grid[x, y].worldPosition, Vector2.one * 1.9f * nodeRadius);
                }
    }

}
