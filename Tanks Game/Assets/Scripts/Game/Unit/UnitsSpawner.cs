using Photon.Pun;
using System.IO;
using UnityEngine;

public class UnitsSpawner : MonoBehaviour
{
    public PhotonPlayer Owner;
    public Spawnpoint[] spawnpoints;

    public void InitUnitsSpawn()
    {
        foreach (Spawnpoint s in spawnpoints)
            SpawnUnit(s);
    }

    public void SpawnUnit(Spawnpoint spawnpoint)
    {
        spawnpoint.unit = PhotonNetwork.Instantiate(
            Path.Combine("Prefabs", "Photon", spawnpoint.unitPrefab.name),
            spawnpoint.transform.position,
            Quaternion.identity).GetComponent<UnitController>();

        spawnpoint.unit.Owner_ID = Owner.Id;
        spawnpoint.unit.Unique_ID = Owner.Id + UnitsManager.Instance.units.Count;
        UnitsManager.Instance.units.Add(spawnpoint.unit.Unique_ID, spawnpoint.unit);
        UnitsManager.Instance.unitsIds.Add(spawnpoint.unit.Unique_ID);

        spawnpoint.unitIsDestroyed = false;
    }
}
