using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using System.Linq;

public class PhotonPlayer : MonoBehaviourPun
{
    public int Id = -1;
    public Player Owner { private set; get; }
    public PhotonView pv { private set; get; }

    public UnitController selectedUnit;

    private UnitsSpawner unitsSpawner;

    void Awake()
    {
        PhotonPeer.RegisterType(typeof(PhotonPlayer), (byte)'L', SerializePhotonPlayer, DeserializePhotonPlayer);
        pv = GetComponent<PhotonView>();
    }

    void Start()
    {
        if (pv.IsMine)
        {
            Owner = pv.Owner;
            Id = Owner.ActorNumber;
            GameManager.Instance.myPP = this;
            // Adding this photon player to list.
            CallInitPhotonPlayer(this);

            unitsSpawner = UnitsManager.Instance.spawners[Id - 1];

            unitsSpawner.Owner = this;
            unitsSpawner.InitUnitsSpawn();
        }
    }

    void Update()
    {
        if (pv.IsMine)
        {
            // Unit selection.
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Node selectedNode = Pathfinding.Instance.gridComponent.GetNodeFromVector2(mousePos);
                if (selectedNode != null && UnitsManager.Instance.units.Any(x => x.Value.CurrentNode == selectedNode))
                {
                    if (selectedUnit != null)
                        selectedUnit.isSelected = false;

                    // If potential unit is mine => select.
                    UnitController potentialUnit = UnitsManager.Instance.units.FirstOrDefault(x => x.Value.CurrentNode == selectedNode).Value;
                    if (potentialUnit.Owner_ID == Id)
                    {
                        SelectUnit(potentialUnit);
                        Pathfinding.Instance.gridComponent.visual.ShowGrid(potentialUnit.CurrentNode);
                    }
                        
                }
            }
        }
    }

    void SelectUnit(UnitController unitToSelect)
    {
        unitToSelect.isSelected = true;
        selectedUnit = unitToSelect;
        //Pathfinding.Instance.
    }

    #region Serialize/Deserialize
    private static byte[] SerializePhotonPlayer(object o)
    {
        PhotonPlayer pp = (PhotonPlayer)o;
        byte[] bytes = new byte[4];
        int index = 0;
        ExitGames.Client.Photon.Protocol.Serialize(pp.Id, bytes, ref index);
        return bytes;
    }

    private static object DeserializePhotonPlayer(byte[] bytes)
    {
        PhotonPlayer pp = new PhotonPlayer();
        int index = 0;
        ExitGames.Client.Photon.Protocol.Deserialize(out pp.Id, bytes, ref index);
        return pp;
    }
    #endregion 

    #region Init
    void CallInitPhotonPlayer(PhotonPlayer pp)
    {
        pv.RPC("InitPhotonPlayer", RpcTarget.AllBuffered, pp);
    }

    [PunRPC]
    void InitPhotonPlayer(PhotonPlayer pp)
    {
        this.Id = pp.Id;
        PhotonRoom.Instance.photonPlayers.Add(pp);
    }
    #endregion
}
