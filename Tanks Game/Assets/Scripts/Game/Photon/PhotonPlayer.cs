using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;

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

            unitsSpawner = UnitsManager.Instance.spawners[PhotonNetwork.CountOfPlayers - 1];

            unitsSpawner.Owner = this;
            foreach (Spawnpoint s in unitsSpawner.spawnpoints)
                unitsSpawner.SpawnUnit(s);

            // Adding this photon player to list with its controller.
            CallInitPhotonPlayer(this);
        }
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
        pv.RPC("InitPhotonPlayer", RpcTarget.OthersBuffered, pp);
    }

    [PunRPC]
    void InitPhotonPlayer(PhotonPlayer pp)
    {
        this.Id = pp.Id;
        PhotonRoom.Instance.photonPlayers.Add(pp);
    }
    #endregion
}
