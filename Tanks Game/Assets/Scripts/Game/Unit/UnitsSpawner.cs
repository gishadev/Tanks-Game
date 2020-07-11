using Photon.Pun;
using System.IO;
using UnityEngine;

public class UnitsSpawner : MonoBehaviour
{
    public PhotonPlayer Owner;
    public Spawnpoint[] spawnpoints;

    public void SpawnUnit(Spawnpoint spawnpoint)
    {
        spawnpoint.unit = PhotonNetwork.Instantiate(
            Path.Combine("Prefabs", "Photon", spawnpoint.unitPrefab.name),
            spawnpoint.transform.position,
            Quaternion.identity).GetComponent<UnitController>();

        spawnpoint.unit.Owner_ID = Owner.Id;

        spawnpoint.unitIsDestroyed = false;
    }
}
