using Photon.Pun;
using System.IO;
using UnityEngine;

public class PhotonPlayer : MonoBehaviour
{
    public PhotonView pv { private set; get; }
    public AvatarController localAvatar { private set; get; }

    void Start()
    {
        pv = GetComponent<PhotonView>();

        if (pv.IsMine)
        {
            // Instantiating player avatar.
            localAvatar = PhotonNetwork.Instantiate(
    Path.Combine("Prefabs", "Photon", GameManager.Instance.playerPrefab.name),
    GameManager.Instance.spawnpoints[Random.Range(0, GameManager.Instance.spawnpoints.Length)].position,
    Quaternion.identity).GetComponent<AvatarController>();

            // Setting camera to player avatar.
            GameManager.Instance.cam.SetTarget(localAvatar.transform);
        }

    }
}
