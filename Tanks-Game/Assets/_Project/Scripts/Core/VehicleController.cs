using System;
using UnityEngine;

namespace Gisha.TanksGame.Core
{
    public class VehicleController : MonoBehaviour
    {
        [Header("Input")]
        [SerializeField] private string hAxis;
        [SerializeField] private string vAxis;
        [SerializeField] private KeyCode shootKey;

        [Header("Components")]
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private Transform shootPos;

        [Header("Speeds")]
        [SerializeField] private float moveSpeed;
        [SerializeField] private float rotationSpeed;

        public event Action OnDestroyed;

        float _hInput;
        float _vInput;
        Rigidbody2D _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            _vInput = Input.GetAxisRaw(vAxis);
            _hInput = Input.GetAxisRaw(hAxis);

            transform.Rotate(-Vector3.forward * _hInput * rotationSpeed * Time.deltaTime);

            if (Input.GetKeyDown(shootKey))
                Shoot();
        }

        private void FixedUpdate()
        {
            _rb.velocity = transform.up * _vInput * moveSpeed * Time.deltaTime;
        }

        private void Shoot()
        {
            var projectile = Instantiate(projectilePrefab, shootPos.position, shootPos.rotation).GetComponent<Projectile>();
            projectile.OwnerTag = gameObject.tag;
        }

        public void DestroyVehicle()
        {
            OnDestroyed?.Invoke();
            Destroy(gameObject);
        }
    }
}
