using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.IO;
using UnityEngine;

public class PhotonPlayer : MonoBehaviourPun
{
    public int Id = -1;
    public Player Owner { private set; get; }
    public PhotonView pv { private set; get; }

    void Awake()
    {
        pv = GetComponent<PhotonView>();
    }

    void Start()
    {
        if (pv.IsMine)
        {
            Owner = pv.Owner;
            Id = Owner.ActorNumber;

            // Adding this photon player to list with its controller.
            CallInitPhotonPlayer(this);
        }
    }

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
