using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;

    PhotonView pv;
    Rigidbody rb;

    void Start()
    {
        pv = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (pv.IsMine)
        {
            Movement();
        }
    }

    void Movement()
    {
        // Movement Input.
        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

        // Actual movement.
        rb.velocity = input * speed;
    }
}
