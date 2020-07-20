using Photon.Pun;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    #region Singleton
    public static UIManager Instance;
    #endregion

    public GameObject shoot_Btn;
    public GameObject cancelShoot_Btn;
    [Space]
    public Transform healthBarsParent;
    public GameObject unitHealthBar;
    [Space]
    public GameObject spectatorModeBtn;
    public GameObject followerModeBtn;
    void Awake()
    {
        Instance = this;
    }

    public void onClick_EndTurn()
    {
        TurnsController.Instance.Next();
    }

    public void onClick_ToSpectatorMode()
    {
        FindObjectOfType<CameraTransform>().isSpectatorMode = true;
        spectatorModeBtn.SetActive(true);
        followerModeBtn.SetActive(false);
    }

    public void onClick_ToFollowerMode()
    {
        FindObjectOfType<CameraTransform>().isSpectatorMode = false;
        spectatorModeBtn.SetActive(false);
        followerModeBtn.SetActive(true);
    }

    public void onClick_Shoot()
    {
        UnitController selectedUnit = GameManager.Instance.myPP.SelectedUnit;
        if (selectedUnit != null)
        {
            if (!selectedUnit.IsMoving)
                selectedUnit.StartShootMode();
        }
    }

    public void onClick_CancelShoot()
    {
        UnitController selectedUnit = GameManager.Instance.myPP.SelectedUnit;
        if (selectedUnit != null)
            selectedUnit.CancelShootMode();
    }

    public void onClick_Disconnent()
    {
        PhotonNetwork.Disconnect();
    }

    public void ShowShootBtn()
    {
        shoot_Btn.SetActive(true);
        cancelShoot_Btn.SetActive(false);
    }
    public void HideShootBtn()
    {
        shoot_Btn.SetActive(false);
        cancelShoot_Btn.SetActive(true);
    }
    public void SpawnUnitHealthBar(UnitController unit)
    {
        HealthBar bar = Instantiate(unitHealthBar,Vector3.zero, Quaternion.identity, healthBarsParent).GetComponent<HealthBar>();
        bar.SetTarget(unit.transform);
        unit.healthBar = bar;
    }
}
