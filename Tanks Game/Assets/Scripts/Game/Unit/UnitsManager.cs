using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class UnitsManager : MonoBehaviour
{
    #region Singleton
    public static UnitsManager Instance { private set; get; }
    #endregion 

    public UnitsSpawner[] spawners { private set; get; }
    // Key is Unique_ID.
    public Dictionary<int, UnitController> units = new Dictionary<int, UnitController>();
    public List<int> unitsIds = new List<int>();

    void Awake()
    {
        Instance = this;

        spawners = FindObjectsOfType<UnitsSpawner>();
    }
}
