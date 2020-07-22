using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    #region Singleton
    public static Pathfinding Instance { private set; get; }
    #endregion

    public int movementRadius;

    [HideInInspector] public PGrid gridComponent;
    void Awake()
    {
        Instance = this;
        gridComponent = GetComponent<PGrid>();
    }

    public List<Node> FindPath(Vector2 startPos, Vector2 endPos)
    {
        Node startNode = gridComponent.GetNodeFromVector2(startPos);
        Node endNode = gridComponent.GetNodeFromVector2(endPos);

        List<Node> openList = new List<Node>(); // List with not evaluated nodes.
        HashSet<Node> closedList = new HashSet<Node>();  // List with evaluated nodes.
        openList.Add(startNode);

        while (openList.Count > 0)
        {
            Node node = openList[0];

            // Checking and closing more profitably nodes among opened.
            for (int i = 1; i < openList.Count; i++)
            {
                if (openList[i].fCost < node.fCost || openList[i].fCost == node.fCost)
                    if (openList[i].hCost < node.hCost)
                        node = openList[i];
            }

            openList.Remove(node);
            closedList.Add(node);

            // If node is end node => form path.
            if (node == endNode)
            {
                return RetracePath(startNode, endNode);
            }

            // Evaluating and opening all the neighbours of node.
            foreach (Node neighbour in gridComponent.GetNeighbours(node))
            {
                if (!neighbour.isWalkable || closedList.Contains(neighbour))
                    continue;

                int newCostToNeighbour = node.gCost + GetDistance(node, neighbour);
                if (newCostToNeighbour < neighbour.gCost || !openList.Contains(neighbour))
                {
                    neighbour.gCost = newCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, endNode);
                    neighbour.parent = node;

                    if (!openList.Contains(neighbour))
                        openList.Add(neighbour);
                }
            }
        }

        // Impossible to calculate the path.
        return null;
    }

    List<Node> RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();

        //  currentPath = path;
        return path;
    }

    int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);

        return 14 * dstX + 10 * (dstY - dstX);
    }

    public List<Node> CalculateAvailableArea(Node center)
    {
        List<Node> result = new List<Node>();

        for (int x = -movementRadius; x <= movementRadius; x++)
            for (int y = -movementRadius; y <= movementRadius; y++)
            {
                int checkX = center.gridX + x;
                int checkY = center.gridY + y;

                // Skipping center node.
                if (checkX == center.gridX && checkY == center.gridY)
                    continue;

                if (checkX > 0 && checkX < gridComponent.gridSizeX && checkY > 0 && checkY < gridComponent.gridSizeY)
                {
                    Node checkNode = gridComponent.grid[checkX, checkY];

                    if (checkNode.isWalkable && GetDistance(checkNode, center) <= movementRadius * 10)
                        result.Add(checkNode);
                }
            }
        return result;
    }
}
