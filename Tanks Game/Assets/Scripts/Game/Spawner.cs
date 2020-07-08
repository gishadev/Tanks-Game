using System.Linq;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public float respawnTime;
    [Tooltip("If there is no players in radius around certain spawnpoint, then this spawnpoint becomes available.")]
    public float checkingRadius;

    public Transform[] spawnpoints;

    public Transform GetSpawnpoint(int index)
    {
        int result = index;

        for (int i = 0; i < result; i++)
        {
            if (result >= spawnpoints.Length)
                result -= spawnpoints.Length;
            else
                break;
        }

        return spawnpoints[result];
    }

    public Transform GetRandomSpawnpoint()
    {
        foreach (Transform s in spawnpoints)
        {
            // Checking for available spawnpoint.
            Collider2D[] colliders = Physics2D.OverlapCircleAll(s.position, checkingRadius);


            if (!colliders.Any(x => x.CompareTag("Player")))
                return s;
        }

        return null;
    }
}
