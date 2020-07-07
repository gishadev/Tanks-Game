using Photon.Pun;
using Photon.Realtime;
using System.IO;
using UnityEngine;

public class PhotonPlayer : MonoBehaviour
{
    [HideInInspector] public Player player;
    public PhotonView pv { private set; get; }
    public PlayerController localController { private set; get; }

    [HideInInspector] public Transform spawnpoint;

    void Start()
    {
        pv = GetComponent<PhotonView>();

        if (pv.IsMine)
        {
            player = pv.Owner;
            // Setting spawnpoint.
            spawnpoint = FindObjectOfType<Spawner>().GetSpawnpoint(player.ActorNumber - 1);

            // Instantiating player avatar.
            localController = PhotonNetwork.Instantiate(
            Path.Combine("Prefabs", "Photon", GameManager.Instance.playerPrefab.name),
            spawnpoint.position,
            Quaternion.identity).GetComponent<PlayerController>();

            // Setting camera on player avatar.
            GameManager.Instance.cam.SetTarget(localController.transform);
        }
    }
}
