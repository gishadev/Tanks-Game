using UnityEngine;

namespace Gisha.TanksGame.Core
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float flySpeed;
        [SerializeField] private float lifeTime;

        [Header("Raycast")]
        [SerializeField] private LayerMask whatIsSolid;
        [SerializeField] private float raycastDistance = 0.1f;

        public string OwnerTag { get; set; }

        private void Start()
        {
            Invoke("DestroyProjectile", lifeTime);

            if (string.IsNullOrEmpty(OwnerTag))
                Debug.LogError("There is no OwnerTag!");
        }

        private void Update()
        {
            transform.Translate(transform.up * flySpeed * Time.deltaTime, Space.World);

            RaycastForSolid();
        }

        private void RaycastForSolid()
        {
            var hitInfo = Physics2D.Raycast(transform.position, transform.up, raycastDistance, whatIsSolid);

            if (hitInfo.collider != null)
            {
                // If hit vehicle is enemy > destroy it.
                if ((hitInfo.collider.CompareTag("Player1") || hitInfo.collider.CompareTag("Player2")) && !hitInfo.collider.CompareTag(OwnerTag))
                    hitInfo.collider.GetComponent<VehicleController>().DestroyVehicle();

                DestroyProjectile();
            }
        }

        private void DestroyProjectile()
        {
            Debug.Log("Projectile was destroyed!");

            Destroy(gameObject);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, transform.up * raycastDistance);
        }
    }
}
