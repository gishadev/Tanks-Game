using Photon.Pun;
using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviourPun
{
    [SerializeField] private float speed;
    [SerializeField] private float lifeTime;

    [SerializeField] private LayerMask whatIsSolid;

    PhotonView pv;

    void Start()
    {
        pv = GetComponent<PhotonView>();

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
                hitInfo.collider.transform.parent.GetComponent<PlayerController>().photonPlayer.CallDie();
            }

            DestroyProj();
        }

    }

    IEnumerator LifeTimeProj()
    {
        while(true)
        {
            yield return new WaitForSeconds(lifeTime);
            DestroyProj();
        }
    }

    //void CallDestroyProj()
    //{
    //    PhotonNetwork.Destroy(gameObject);
    //    pv.RPC("DestroyProj", RpcTarget.All);
    //}

    [PunRPC]
    void DestroyProj()
    {
        PhotonNetwork.Destroy(gameObject);
    }
}
