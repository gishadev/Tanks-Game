using ExitGames.Client.Photon;
using Photon.Pun;
using System.Collections;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    public int Owner_ID = -1;
    public int Unique_ID = -1;
    [Space]
    public bool isSelected;
    public PhotonPlayer Owner;
    public Node CurrentNode
    {
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

    public PhotonView pv;
    Animator animator;
    [HideInInspector] public UnitMovement movement;
    Camera cam;

    void Awake()
    {
        PhotonPeer.RegisterType(typeof(UnitController), (byte)'H', SerializeUnitController, DeserializeUnitController);

        pv = GetComponent<PhotonView>();
        animator = GetComponent<Animator>();
        movement = GetComponent<UnitMovement>();
        cam = Camera.main;

    }

    void Start()
    {
        if (pv.IsMine)
        {
            InitUnitController();
        }
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
                    if (!Pathfinding.Instance.grid.IsBlockedWithUnit(Pathfinding.Instance.grid.GetNodeFromVector2(destination)) && movement.PathIsDone)
                        pv.RPC("StartUnitMovement", RpcTarget.All, destination);
                }
            }
        }
    }

    #region Serialize/Deserialize
    private static byte[] SerializeUnitController(object o)
    {
        UnitController uc = (UnitController)o;
        byte[] bytes = new byte[8];
        int index = 0;
        ExitGames.Client.Photon.Protocol.Serialize(uc.Owner_ID, bytes, ref index);
        ExitGames.Client.Photon.Protocol.Serialize(uc.Unique_ID, bytes, ref index);
        return bytes;
    }

    private static object DeserializeUnitController(byte[] bytes)
    {
        UnitController uc = new UnitController();
        int index = 0;
        ExitGames.Client.Photon.Protocol.Deserialize(out uc.Owner_ID, bytes, ref index);
        ExitGames.Client.Photon.Protocol.Deserialize(out uc.Unique_ID, bytes, ref index);
        return uc;
    }
    #endregion

    #region Init
    void InitUnitController()
    {
        // Client Side data of the unit.
        Owner_ID = Owner.Id;
        Unique_ID = pv.InstantiationId;
        UnitsManager.Instance.units.Add(Unique_ID, this);
        UnitsManager.Instance.unitsIds.Add(Unique_ID);

        // Transfering Data of this unit controller to others clients.
        pv.RPC("TransferUnitData", RpcTarget.OthersBuffered, this);
    }

    [PunRPC]
    void TransferUnitData(UnitController unit)
    {
        this.Unique_ID = unit.Unique_ID;
        this.Owner_ID = unit.Owner_ID;

        UnitsManager.Instance.unitsIds.Add(this.Unique_ID);
        UnitsManager.Instance.units.Add(this.Unique_ID, this);
    }
    #endregion

    [PunRPC]
    void StartUnitMovement(Vector2 destination)
    {
        movement.StartMovement(destination);
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
