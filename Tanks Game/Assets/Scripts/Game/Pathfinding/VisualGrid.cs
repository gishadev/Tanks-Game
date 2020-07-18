using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class VisualGrid
{
    [Header("Visual")]
    public GameObject visualTilePrefab;
    public Transform visualParent;

    GameObject[,] visualGrid;

    public void CreateGrid(PGrid gridComponent)
    {
        visualGrid = new GameObject[gridComponent.gridSizeX, gridComponent.gridSizeY];

        for (int x = 0; x < gridComponent.gridSizeX; x++)
            for (int y = 0; y < gridComponent.gridSizeY; y++)
                visualGrid[x, y] = GameObject.Instantiate(visualTilePrefab, gridComponent.grid[x, y].worldPosition, Quaternion.identity, visualParent);

        DeactivateGrid();
    }

    public void ShowGrid(Node node)
    {
        DeactivateGrid();
        List<Node> availableArea = Pathfinding.Instance.CalculateAvailableArea(node);

        foreach (Node n in availableArea)
        {
            visualGrid[n.gridX, n.gridY].SetActive(true);
        }
    }

    void DeactivateGrid()
    {
        for (int x = 0; x < visualGrid.GetLength(0); x++)
            for (int y = 0; y < visualGrid.GetLength(1); y++)
            {
                visualGrid[x, y].SetActive(false);
            }
    }
}
