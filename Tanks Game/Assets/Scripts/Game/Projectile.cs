using Photon.Pun;
using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float lifeTime;

    PhotonView pv;

    void Start()
    {
        pv = GetComponent<PhotonView>();

        StartCoroutine(DestroyProj());
    }

    void Update()
    {
        // Projectile Movement.
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    IEnumerator DestroyProj()
    {
        while(true)
        {
            yield return new WaitForSeconds(lifeTime);
            pv.RPC("destroy", RpcTarget.All);
        }
    }

    [PunRPC]
    void destroy()
    {
        Destroy(gameObject);
    }
}
