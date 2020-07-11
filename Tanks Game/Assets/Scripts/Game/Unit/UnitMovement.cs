using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMovement : MonoBehaviour
{
    List<Node> path = new List<Node>();

    public void StartMovement(Vector2 destination)
    {
        Pathfinding.Instance.FindPath(transform.position, destination);

        StartCoroutine(GetPath());
    }

    IEnumerator GetPath()
    {
        while (true)
        {
            if (Pathfinding.Instance.isCalculated)
            {
                path = Pathfinding.Instance.currentPath;
                StartCoroutine(Movement());
                break;
            }

            yield return null;
        }
    }

    IEnumerator Movement()
    {
        while (path.Count > 0)
        {
            transform.position = path[0].worldPosition;
            path.RemoveAt(0);
            yield return new WaitForSeconds(0.25f);
        }
    }
}
