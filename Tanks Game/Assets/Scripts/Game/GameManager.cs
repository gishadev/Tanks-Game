using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager Instance { private set; get; }
    #endregion
    public GameObject playerPrefab;

    public Transform[] spawnpoints;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Instantiate(playerPrefab, spawnpoints[Random.Range(0, spawnpoints.Length)]);
    }
}
