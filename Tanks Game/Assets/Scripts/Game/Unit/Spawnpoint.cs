using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Spawnpoint : MonoBehaviour
{
    public bool unitIsDestroyed = true;

    public GameObject unitPrefab;
    public UnitController unit;

    // Initializing spawning.
    public void Spawn(PhotonPlayer Owner)
    {
        unit = PhotonNetwork.Instantiate(
        Path.Combine("Prefabs", "Photon", unitPrefab.name),
        transform.position,
        Quaternion.identity).GetComponent<UnitController>();

        unit.Owner = Owner;

        unitIsDestroyed = false;
    }
}
