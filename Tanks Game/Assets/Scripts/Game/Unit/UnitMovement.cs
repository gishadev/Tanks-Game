using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMovement : MonoBehaviour
{
    public Node currentNode;

    void Start()
    {
        currentNode = Pathfinding.Instance.grid.GetNodeFromVector2(transform.position);
    }

    public void StartMovement(Vector2 destination)
    {
        List<Node> path = Pathfinding.Instance.FindPath(transform.position, destination);

        if (path != null)
            StartCoroutine(Movement(Pathfinding.Instance.FindPath(transform.position, destination)));
    }

    IEnumerator Movement(List<Node> _path)
    {
        List<Node> path = _path;
        while (path.Count > 0)
        {
            currentNode = path[0];
            transform.position = currentNode.worldPosition;
            path.RemoveAt(0);
            yield return new WaitForSeconds(0.25f);
        }
    }
}
