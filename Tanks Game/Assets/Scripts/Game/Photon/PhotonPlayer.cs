using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class PhotonPlayer : MonoBehaviourPun
{
    public int Id = -1;
    public Player Owner { private set; get; }
    public PhotonView pv { private set; get; }

    public List<UnitController> myUnits = new List<UnitController>();
    public UnitController SelectedUnit { private set; get; }

    private UnitsSpawner unitsSpawner;

    void Awake()
    {
        PhotonPeer.RegisterType(typeof(PhotonPlayer), (byte)'L', SerializePhotonPlayer, DeserializePhotonPlayer);
        pv = GetComponent<PhotonView>();

        if (pv.IsMine)
        {
            Owner = pv.Owner;
            Id = Owner.ActorNumber;
            GameManager.Instance.myPP = this;
            // Adding this photon player to list.
            CallInitPhotonPlayer(this);

            // Setting Units Spawner.
            unitsSpawner = UnitsManager.Instance.spawners[Id - 1];
            unitsSpawner.Owner = this;
            unitsSpawner.InitUnitsSpawn();
        }
    }
    public void SelectUnit(UnitController unitToSelect)
    {
        if (SelectedUnit != null)
        {
            SelectedUnit.isSelected = false;
            SelectedUnit.selectedMarker.SetActive(false);
        }
            
        unitToSelect.isSelected = true;
        unitToSelect.selectedMarker.SetActive(true);
        SelectedUnit = unitToSelect;
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
