using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region Singleton
    public static UIManager Instance;
    #endregion

    public Button shootBtn;
    public Button cancelShootBtn;

    public Button moveBtn;
    public Button cancelMoveBtn;
    public Button endTurnBtn;
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

    #region Buttons
    public void onClick_EndTurn()
    {
        UnitController selectedUnit = GameManager.Instance.NowMyUnitController;
        if (GameManager.Instance.NowMyUnitController != null)
        {
            if (!selectedUnit.IsMoving)
                TurnsController.Instance.CallNext();
        }
    }

    #region Camera
    public void onClick_ToFreeflyMode()
    {
        FindObjectOfType<CameraTransform>().ChangeCameraMode(CameraTransform.CameraModes.Freefly);
    }

    public void onClick_ToFollowingMode()
    {
        FindObjectOfType<CameraTransform>().ChangeCameraMode(CameraTransform.CameraModes.Following);
    }

    public void ShowFreeflyBtn()
    {
        spectatorModeBtn.SetActive(true);
        followerModeBtn.SetActive(false);
    }

    public void ShowFollowingBtn()
    {
        spectatorModeBtn.SetActive(false);
        followerModeBtn.SetActive(true);
    }
    #endregion Camera

    #region Shoot
    public void onClick_Shoot()
    {
        UnitController selectedUnit = GameManager.Instance.NowMyUnitController;
        if (selectedUnit == null)
            return;

        if (!selectedUnit.IsMoving)
        {
            selectedUnit.isShootMode = true;
            selectedUnit.isMoveMode = false;
            StartCoroutine(selectedUnit.ShootMode());
            HideShootBtn();
            BlockButtons();
        }
    }

    public void onClick_CancelShoot()
    {
        UnitController selectedUnit = GameManager.Instance.myPP.SelectedUnit;
        if (selectedUnit == null)
            return;

        selectedUnit.isShootMode = false;
        StartCoroutine(selectedUnit.ReturnTurretToInitState());
        ShowShootBtn();
        UnblockButtons();
    }

    public void ShowShootBtn()
    {
        shootBtn.gameObject.SetActive(true);
        cancelShootBtn.gameObject.SetActive(false);
    }

    public void HideShootBtn()
    {
        shootBtn.gameObject.SetActive(false);
        cancelShootBtn.gameObject.SetActive(true);
    }

    #endregion Shoot

    #region Move
    public void onClick_Move()
    {
        UnitController selectedUnit = GameManager.Instance.NowMyUnitController;
        if (selectedUnit == null)
            return;

        if (!selectedUnit.IsMoving)
        {
            selectedUnit.isMoveMode = true;
            Pathfinding.Instance.gridComponent.visual.ShowGrid(selectedUnit.CurrentNode);
            HideMoveBtn();
            BlockButtons();
        }

    }
    public void onClick_CancelMove()
    {
        UnitController selectedUnit = GameManager.Instance.myPP.SelectedUnit;
        if (selectedUnit == null)
            return;

        Pathfinding.Instance.gridComponent.visual.HideGrid();
        selectedUnit.isMoveMode = false;
        ShowMoveBtn();
        UnblockButtons();
    }

    public void ShowMoveBtn()
    {
        moveBtn.gameObject.SetActive(true);
        cancelMoveBtn.gameObject.SetActive(false);
    }

    public void HideMoveBtn()
    {
        moveBtn.gameObject.SetActive(false);
        cancelMoveBtn.gameObject.SetActive(true);
    }
    #endregion Move

    public void onClick_Disconnent()
    {
        PhotonNetwork.Disconnect();
    }

    public void ResetButtons()
    {
        ShowShootBtn();
        ShowMoveBtn();

        UnblockButtons();
    }

    public void BlockButtons()
    {
        shootBtn.interactable = false;
        moveBtn.interactable = false;
        endTurnBtn.interactable = false;
    }

    void UnblockButtons()
    {
        shootBtn.interactable = true;
        moveBtn.interactable = true;
        endTurnBtn.interactable = true;
    }

    #endregion

    public void SpawnUnitHealthBar(UnitController unit)
    {
        HealthBar bar = Instantiate(unitHealthBar, Vector3.zero, Quaternion.identity, healthBarsParent).GetComponent<HealthBar>();
        bar.SetTarget(unit.transform);
        unit.healthBar = bar;
    }
}
