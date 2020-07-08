using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.IO;
using UnityEngine;

public class PhotonPlayer : MonoBehaviourPun
{
    public int Id = -1;
    [HideInInspector] public Player player;
    public PhotonView pv { private set; get; }
    [SerializeField] public PlayerController localController { set; get; }

    [HideInInspector] public Transform initSpawnpoint;
    Spawner spawner;

    void Awake()
    {
        PhotonPeer.RegisterType(typeof(PhotonPlayer), (byte)'L', SerializePhotonPlayer, DeserializePhotonPlayer);
    }

    void Start()
    {
        pv = GetComponent<PhotonView>();
        spawner = FindObjectOfType<Spawner>();

        if (pv.IsMine)
        {
            player = pv.Owner;
            Id = player.ActorNumber;

            // Adding this photon player to list with its controller.
            CallInitPhotonPlayer(this);

            // Setting spawnpoint for player controller.
            Transform spawnpoint = FindObjectOfType<Spawner>().GetSpawnpoint(player.ActorNumber - 1);

            // Instantiating player avatar.
            localController = PhotonNetwork.Instantiate(
            Path.Combine("Prefabs", "Photon", GameManager.Instance.playerPrefab.name),
            spawnpoint.position,
            Quaternion.identity).GetComponent<PlayerController>();

            localController.Id = Id;
            CallSetController(Id);

            initSpawnpoint = spawnpoint;
            localController.photonPlayer = this;
            // Setting camera on player avatar.
            GameManager.Instance.cam.SetTarget(localController.transform);
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
        Debug.Log(PhotonRoom.Instance.photonPlayers.Count);
    }

    void CallSetController(int id)
    {
        pv.RPC("SetController", RpcTarget.OthersBuffered, id);
    }
    [PunRPC]
    void SetController(int id)
    {
        PlayerController controller;

        foreach (PhotonView localPV in FindObjectsOfType<PhotonView>())
        {
            if (localPV.Owner.ActorNumber == Id)
            {
                if (localPV.TryGetComponent<PlayerController>(out controller))
                {
                    localController = controller;
                    localController.Id = id;
                    localController.photonPlayer = this;
                    break;
                }    
            }
        }
    }
    #endregion

    public void CallDie()
    {
        if (pv.IsMine)
            this.pv.RPC("Die", RpcTarget.All);
    }

    [PunRPC]
    void Die()
    {
        localController.gameObject.SetActive(false);

        StartCoroutine(Respawning());
    }

    IEnumerator Respawning()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawner.respawnTime);

            // Getting available spawnpoint.
            Vector2 randomPosition;
            if (spawner.GetRandomSpawnpoint() != null)
                randomPosition = spawner.GetRandomSpawnpoint().position;
            else
                randomPosition = initSpawnpoint.position;

            localController.transform.position = randomPosition;
            localController.isReadyToShoot = true;
            localController.gameObject.SetActive(true);

            break;
        }
    }
}
