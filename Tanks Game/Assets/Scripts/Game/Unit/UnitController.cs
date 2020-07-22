using ExitGames.Client.Photon;
using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
public class UnitController : MonoBehaviour
{
    public string Name;
    [Space]
    public int Owner_ID = -1;
    public int Unique_ID = -1;
    [Space]
    public bool isSelected = false;
    public bool isShootMode = false;
    public bool isMoveMode = false;
    public bool IsMoving { get { return !Movement.PathIsDone; } }
    [Space]
    public GameObject selectedMarker;
    [Header("Health")]
    [SerializeField] private int nowHealth = 3;
    [SerializeField] private int maxHealth = 3;
    public int Health
    {
        get { return nowHealth; }
        set
        {
            if (value > maxHealth)
                nowHealth = maxHealth;
            else if (value <= 0)
                nowHealth = 0;
            else
                nowHealth = value;

            healthBar.UpdatePercentage(nowHealth, maxHealth);
        }
    }

    public HealthBar healthBar;
    [Header("Shoot Mode")]
    public GameObject turret;
    public GameObject projPrefab;
    public Transform shootPos;
    public float shootDelay = 2f;

    public PhotonView pv;

    Animator animator;
    UnitMovement movement;
    Camera cam;

    public UnitMovement Movement { get { return movement; } }

    public Node CurrentNode
    {
        get
        {
            return Movement.currentNode;
        }
    }

    public PhotonPlayer Owner
    {
        get
        {
            int index = PhotonRoom.Instance.photonPlayers.FindIndex(x => x.Id == Owner_ID);
            if (index != -1)
                return PhotonRoom.Instance.photonPlayers[index];
            else
            {
                Debug.LogError("Photon Player was not found!");
                return null;
            }  
        }
    }

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
        UIManager.Instance.SpawnUnitHealthBar(this);
        if (pv.IsMine)
        {
            InitUnitController();
        }
    }

    void Update()
    {
        if (pv.IsMine)
        {
            if (isSelected && !isShootMode && isMoveMode)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    // If player's pointer is on UI element (Button).
                    if (EventSystem.current.IsPointerOverGameObject())
                        return;

                    Node destinationNode = Pathfinding.Instance.gridComponent.GetNodeFromVector2(GetCursorWorldPosition());
                    // Unit can move in certain radius around it.
                    bool isInAvailableArea = Pathfinding.Instance.CalculateAvailableArea(CurrentNode).Contains(destinationNode);

                    if (!IsMoving && isInAvailableArea)
                        pv.RPC("StartUnitMovement", RpcTarget.All, destinationNode.worldPosition);
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
        Unique_ID = pv.InstantiationId;
        UnitsManager.Instance.units.Add(Unique_ID, this);
        UnitsManager.Instance.unitsIds.Add(Unique_ID);

        // Transfering Data of this unit controller to others clients.
        pv.RPC("TransferUnitData", RpcTarget.OthersBuffered, this);
    }
    #endregion

    #region PUN
    [PunRPC]
    void StartUnitMovement(Vector2 destination)
    {
        Movement.StartMovement(destination);
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

    #region Shoot Mode
    public IEnumerator ShootMode()
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
                UIManager.Instance.ShowShootBtn();
                isShootMode = false;
                break;
            }

            yield return null;
        }
    }

    public IEnumerator ReturnTurretToInitState()
    {
        while (turret.transform.rotation != transform.rotation)
        {
            turret.transform.rotation = Quaternion.RotateTowards(turret.transform.rotation, transform.rotation, Movement.rotationSpeed * Time.deltaTime);
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

        StartCoroutine(TurnsController.Instance.WaitTillProjectileDestroy(proj));
        StartCoroutine(ReturnTurretToInitState());
    }

    // For Animator.
    public void SetShootToFalse()
    {
        animator.SetBool("Shoot", false);
    }
    #endregion

    #region Taking Damage
    public void TakeDamage()
    {
        Health--;

        if (Health == 0)
            Die();
    }

    void Die()
    {
        // Collections managment.
        int index = UnitsManager.Instance.unitsIds.FindIndex(x => x == Unique_ID);
        if (index != -1)
            UnitsManager.Instance.unitsIds.RemoveAt(index);

        UnitsManager.Instance.units.Remove(Unique_ID);
        Owner.myUnits.Remove(this);

        // Destroying.
        Destroy(healthBar.gameObject);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        Pathfinding.Instance.gridComponent.UpdateGrid();
    }
    #endregion

    Vector2 GetCursorWorldPosition()
    {
        return cam.ScreenToWorldPoint(Input.mousePosition);
    }
}
