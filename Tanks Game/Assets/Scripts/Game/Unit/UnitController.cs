using Photon.Pun;
using System.Collections;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    public int Id = -1;

    [HideInInspector] public PhotonPlayer photonPlayer;

    public bool isReadyToShoot = true;

    public GameObject projPrefab;
    public Transform shootPos;

    [SerializeField] private float shootDelay = 2f;

    PhotonView pv;
    Animator animator;

    void Start()
    {
        pv = GetComponent<PhotonView>();
        animator = GetComponent<Animator>();
    }

    //void Update()
    //{
    //    if (pv.IsMine)
    //    {
    //        if (Input.GetMouseButtonDown(0) && isReadyToShoot)
    //        {
    //            CallShoot();
    //            isReadyToShoot = false;
    //            StartCoroutine(Delay());
    //        }
    //    }
    //}

    //void CallShoot()
    //{
    //    pv.RPC("Shoot", RpcTarget.All, Time.time, shootPos.up, Id);
    //    animator.SetBool("Shoot", true);
    //}

    //[PunRPC]
    //void Shoot(float time, Vector3 up, int ownerId)
    //{
    //    float latency = time / Time.time;
    //    Vector2 position = shootPos.position - (-up * latency) - up;

    //    Projectile proj = Instantiate(projPrefab, position, shootPos.rotation).GetComponent<Projectile>();
    //    proj.ownerId = ownerId;

    //    Debug.Log("latency: " + latency);
    //}

    //// For Animator.
    //public void SetShootToFalse()
    //{
    //    animator.SetBool("Shoot", false);
    //}

    //IEnumerator Delay()
    //{
    //    while (isReadyToShoot == false)
    //    {
    //        yield return new WaitForSeconds(shootDelay);
    //        isReadyToShoot = true;
    //    }
    //}

    //public void TakeDamage()
    //{
    //    photonPlayer.CallDie();
    //}
}
