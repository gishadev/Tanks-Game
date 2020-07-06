using Photon.Pun;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float acceleration;
    [SerializeField] float maxSpeed;

    [SerializeField] GameObject turret;
    [SerializeField] GameObject body;

    Vector2 moveInput;
    Vector2 rawInput;

    PhotonView pv;
    Rigidbody2D rb;

    void Start()
    {
        pv = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (pv.IsMine)
        {
            // Input for movement.
            moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

            // Input for body rotation.
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
                rawInput = moveInput;

            TurretRotation();
            BodyRotation();
        }
    }

    void FixedUpdate()
    {
        if (pv.IsMine)
        {
            Movement();
        }
    }

    void Movement()
    {
        if (moveInput.magnitude < 0.15f)
        {
            rb.velocity = Vector2.zero;
            return;
        }
            
        rb.AddForce(moveInput.normalized * acceleration);

        // Limiting velocity
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }

        Debug.Log(rb.velocity.magnitude);
    }

    void BodyRotation()
    {
        float rotZ = Mathf.Atan2(rawInput.y, rawInput.x) * Mathf.Rad2Deg - 90f;
        body.transform.rotation = Quaternion.Euler(Vector3.forward * rotZ);
    }
    void TurretRotation()
    {
        Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        float rotZ = Mathf.Atan2(cursorPos.y, cursorPos.x) * Mathf.Rad2Deg - 90f;
        turret.transform.rotation = Quaternion.Euler(Vector3.forward * rotZ);
    }
}
