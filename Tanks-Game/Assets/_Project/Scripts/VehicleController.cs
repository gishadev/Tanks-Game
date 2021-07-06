using UnityEngine;

namespace Gisha.TanksGame.Core
{
    public class VehicleController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private float rotationSpeed;

        float _hInput;
        float _vInput;
        Rigidbody2D _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            _vInput = Input.GetAxisRaw("Vertical");
            _hInput = Input.GetAxisRaw("Horizontal");

            transform.Rotate(-Vector3.forward * _hInput * rotationSpeed * Time.deltaTime);
        }

        private void FixedUpdate()
        {
            _rb.velocity = transform.up * _vInput * moveSpeed * Time.deltaTime;
        }
    }
}
