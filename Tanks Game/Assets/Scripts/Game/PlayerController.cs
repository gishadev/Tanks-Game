using Photon.Pun;
using System.Collections;
using System.IO;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private bool isReadyToShoot = true;

    public GameObject projPrefab;
    public Transform shootPos;

    [SerializeField] private float shootDelay = 2f;


    PhotonView pv;

    void Start()
    {
        pv = GetComponent<PhotonView>();
    }

    void Update()
    {
        if (pv.IsMine)
        {
            if (Input.GetMouseButtonDown(0) && isReadyToShoot)
                Shoot();
        }
    }

    void Shoot()
    {
        PhotonNetwork.Instantiate(Path.Combine("Prefabs", "Photon", projPrefab.name), shootPos.position, shootPos.rotation);

        isReadyToShoot = false;
        StartCoroutine(Delay());
    }

    IEnumerator Delay()
    {
        while (true)
        {
            yield return new WaitForSeconds(shootDelay);
            isReadyToShoot = true;
        }
    }
}
