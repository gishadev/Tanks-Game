using Photon.Pun;
using System.IO;
using UnityEngine;

public class UnitsSpawner : MonoBehaviour
{
    public PhotonPlayer Owner;
    public Spawnpoint[] spawnpoints;

    // First Spawning of units.
    public void InitUnitsSpawn()
    {
        foreach (Spawnpoint s in spawnpoints)
        {
            s.Spawn(Owner);
        }
    }

    //// Spawn unit at certain spawnpoint.
    //public void SpawnUnit(Spawnpoint spawnpoint)
    //{
    //    spawnpoint.Spawn(Owner);
    //}
}
