using Photon.Pun;
using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviourPun
{
    [HideInInspector] public float ownerId;
    [SerializeField] public float speed;
    [SerializeField] private float lifeTime;

    [SerializeField] private LayerMask whatIsSolid;

    public GameObject explosionEffect;

    void Start()
    {
        StartCoroutine(LifeTimeProj());
    }

    void Update()
    {
        // Projectile Movement.
        transform.Translate(Vector2.up * speed * Time.deltaTime);

        // Raycasting.
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, Vector2.up, 0.15f, whatIsSolid);

        if (hitInfo.collider != null)
        {
            if (hitInfo.collider.CompareTag("Player"))
            {
                UnitController controller = hitInfo.collider.GetComponentInParent<UnitController>();

                if (ownerId == controller.Unique_ID)
                    return;
                //else
                //    controller.TakeDamage();
            }
            DestroyProj();
        }

    }

    IEnumerator LifeTimeProj()
    {
        while (true)
        {
            yield return new WaitForSeconds(lifeTime);
            DestroyProj();
        }
    }

    void DestroyProj()
    {
        Instantiate(explosionEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
