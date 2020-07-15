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

    public void onClick_Shoot()
    {
        UnitController selectedUnit = myPP.selectedUnit;
        if (selectedUnit != null)
        {
            if (!selectedUnit.IsMoving)
                selectedUnit.StartShootMode();
        }
    }

    public void onClick_CancelShoot()
    {
        UnitController selectedUnit = myPP.selectedUnit;
        if (selectedUnit != null)
        {
                selectedUnit.ExitShootMode();
        }
    }

    public void onClick_Disconnent()
    {
        PhotonNetwork.Disconnect();
    }
}
