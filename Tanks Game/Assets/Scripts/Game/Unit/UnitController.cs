using Photon.Pun;
using System.Collections;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    public int Owner_ID = -1;
    public int Unique_ID = -1;

    public bool isSelected;
    [HideInInspector] public PhotonPlayer photonPlayer;
    public Node CurrentNode { 
        get
        {
           return movement.currentNode;
        }
    }

    [Header("Shoot")]
    public bool isReadyToShoot = true;
    public GameObject projPrefab;
    public Transform shootPos;
    public float shootDelay = 2f;

    PhotonView pv;
    Animator animator;
    [HideInInspector] public UnitMovement movement;
    Camera cam;
    void Start()
    {
        pv = GetComponent<PhotonView>();
        animator = GetComponent<Animator>();
        movement = GetComponent<UnitMovement>();
        cam = Camera.main;
    }

    void Update()
    {
        if (pv.IsMine)
        {
            if (isSelected)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Vector2 destination = cam.ScreenToWorldPoint(Input.mousePosition);
                    if (!Pathfinding.Instance.grid.IsBlockedWithUnit(Pathfinding.Instance.grid.GetNodeFromVector2(destination)))
                        movement.StartMovement(destination);
                }
            }
        }
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
