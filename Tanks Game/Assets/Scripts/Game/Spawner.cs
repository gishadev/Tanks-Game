using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnpoints;

    public Transform GetSpawnpoint(int index)
    {
        int result = index;

        for(int i = 0; i < result; i++)
        {
            if (result >= spawnpoints.Length)
                result -= spawnpoints.Length;
            else
                break;
        }

        return spawnpoints[result];
    }

}
