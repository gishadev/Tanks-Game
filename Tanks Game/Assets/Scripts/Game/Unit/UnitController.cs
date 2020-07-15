using ExitGames.Client.Photon;
using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UnitController : MonoBehaviour
{
    public int Owner_ID = -1;
    public int Unique_ID = -1;
    [Space]

    public bool isSelected = false;
    public bool isShootMode = false;
    public PhotonPlayer Owner;
    public Node CurrentNode
    {
        get
        {
            return movement.currentNode;
        }
    }

    public bool IsMoving { get { return !movement.PathIsDone; } }

    [Header("Shoot")]
    public GameObject turret;
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
            if (isSelected && !isShootMode)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    // If player's pointer is on UI element (Button).
                    if (EventSystem.current.IsPointerOverGameObject())
                        return;

                    Vector2 destination = GetCursorWorldPosition();
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

    #region Shoot
    public void StartShootMode()
    {
        isShootMode = true;
        StartCoroutine(ShootMode());
        UIManager.Instance.HideShootBtn();
    }

    public void ExitShootMode()
    {
        isShootMode = false;
        StartCoroutine(ReturnTurretToInitState());
        UIManager.Instance.ShowShootBtn();
    }

    IEnumerator ShootMode()
    {
        while (isShootMode)
        {
            // Turret turning.
            Vector2 cursorPos = GetCursorWorldPosition() - (Vector2)transform.position;
            float rotZ = Mathf.Atan2(cursorPos.y, cursorPos.x) * Mathf.Rad2Deg - 90f;
            turret.transform.rotation = Quaternion.Euler(Vector3.forward * rotZ);

            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                CallShoot(rotZ);
                ExitShootMode();
                break;
            }

            yield return null;
        }
    }

    IEnumerator ReturnTurretToInitState()
    {
        Debug.Log(turret.transform.rotation.eulerAngles + " " + transform.rotation.eulerAngles);
        while (turret.transform.rotation != transform.rotation)
        {
            turret.transform.rotation = Quaternion.RotateTowards(turret.transform.rotation, transform.rotation, movement.rotationSpeed * Time.deltaTime);
            yield return null;
        }
    }

    void CallShoot(float rotZ)
    {
        pv.RPC("Shoot", RpcTarget.All, rotZ, Unique_ID);
        animator.SetBool("Shoot", true);
    }
    [PunRPC]
    void Shoot(float rotZ, int ownerId)
    {
        turret.transform.rotation = Quaternion.Euler(Vector3.forward * rotZ);

        Projectile proj = Instantiate(projPrefab, shootPos.position, shootPos.rotation).GetComponent<Projectile>();
        proj.ownerId = ownerId;
    }

    // For Animator.
    public void SetShootToFalse()
    {
        animator.SetBool("Shoot", false);
    }
    #endregion

    public void TakeDamage()
    {
        Die();
    }

    void Die()
    {
        // Collections managment.
        int index = UnitsManager.Instance.unitsIds.FindIndex(x => x == Unique_ID);
        if (index != -1)
            UnitsManager.Instance.unitsIds.RemoveAt(index);
        UnitsManager.Instance.units.Remove(Unique_ID);

        // Destroying.
        Destroy(gameObject);
    }

    Vector2 GetCursorWorldPosition()
    {
        return cam.ScreenToWorldPoint(Input.mousePosition);
    }
}
