﻿using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 1.5f;
    [SerializeField] private float rotationSpeed = 15f;
    public Node currentNode { private set; get; }

    Vector2 lookDirection { get { return transform.TransformDirection(Vector2.up); } }

    public List<Node> currentPath = new List<Node>();


    void Start()
    {
        currentNode = Pathfinding.Instance.grid.GetNodeFromVector2(transform.position);
    }

    //void Update()
    //{
    //    Debug.DrawRay(transform.position, GetDirectionToTarget(currentNode.worldPosition) * 10f, Color.red);
    //    Debug.DrawRay(transform.position, lookDirection * 10f, Color.green);
    //}

    public void StartMovement(Vector2 destination)
    {
        List<Node> path = Pathfinding.Instance.FindPath(transform.position, destination);

        if (path != null)
            StartCoroutine(Movement(Pathfinding.Instance.FindPath(transform.position, destination)));
    }

    bool IsCompletedMoveToNext = false;
    bool IsCompletedRotationToNext = false;

    [HideInInspector]
    public bool PathIsDone
    {
        get
        {
            return currentPath.Count == 0;
        }
    }

    IEnumerator Movement(List<Node> _path)
    {
        currentPath = _path;
        while (currentPath.Count > 0)
        {
            IsCompletedMoveToNext = false;
            IsCompletedRotationToNext = false;
            Node nextNode = currentPath[0];

            // Rotate Towards Next Node.
            if (lookDirection != GetDirectionToTarget(nextNode.worldPosition))
                StartCoroutine(RotateTowardsNextNode(nextNode));
            else
                IsCompletedRotationToNext = true;

            yield return new WaitUntil(() => IsCompletedRotationToNext);
            // Move Towards Next Node.
            StartCoroutine(MoveTowardsNextNode(nextNode));
            yield return new WaitUntil(() => IsCompletedMoveToNext);
            currentPath.RemoveAt(0);
            yield return null;
        }
    }

    IEnumerator MoveTowardsNextNode(Node nextNode)
    {
        while (!IsCompletedMoveToNext)
        {
            if (Vector2.Distance(transform.position, nextNode.worldPosition) == 0)
            {
                currentNode = nextNode;
                IsCompletedMoveToNext = true;
                break;
            }

            transform.position = Vector2.MoveTowards(transform.position, nextNode.worldPosition, Time.deltaTime * movementSpeed);

            yield return null;
        }
    }

    IEnumerator RotateTowardsNextNode(Node nextNode)
    {
        while (!IsCompletedRotationToNext)
        {
            Vector2 directionToTarget = GetDirectionToTarget(nextNode.worldPosition);
            if (directionToTarget == lookDirection)
            {
                IsCompletedRotationToNext = true;
                break;
            }

            float rotZ = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;
            Quaternion to = Quaternion.Euler(0f, 0f, rotZ - 90f);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, to, Time.deltaTime * rotationSpeed);

            yield return null;
        }
    }

    Vector2 GetDirectionToTarget(Vector2 target)
    {
        return (target - (Vector2)transform.position).normalized;
    }
}
