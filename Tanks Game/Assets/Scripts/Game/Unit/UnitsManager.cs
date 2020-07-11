using UnityEngine;

public class UnitsManager : MonoBehaviour
{
    #region Singleton
    public static UnitsManager Instance { private set; get; }
    #endregion 

    public UnitsSpawner[] spawners { private set; get; }

    void Awake()
    {
        Instance = this;

        spawners = FindObjectsOfType<UnitsSpawner>();
    }
}
