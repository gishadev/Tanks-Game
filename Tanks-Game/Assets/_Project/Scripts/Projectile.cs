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

        private void Start()
        {
            Invoke("DestroyProjectile", lifeTime);
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
                DestroyProjectile();
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
