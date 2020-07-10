using ExitGames.Client.Photon;
using Photon.Pun;
using UnityEngine;

public class MyCustomTypes : MonoBehaviourPun
{
    void Awake()
    {
        PhotonPeer.RegisterType(typeof(PhotonPlayer), (byte)'L', SerializePhotonPlayer, DeserializePhotonPlayer);
    }

    #region Photon Player
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
}
