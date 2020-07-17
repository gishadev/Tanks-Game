using Photon.Pun;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager Instance { private set; get; }
    #endregion
    public PhotonPlayer myPP;

    void Awake()
    {
        Instance = this;
    }
}
