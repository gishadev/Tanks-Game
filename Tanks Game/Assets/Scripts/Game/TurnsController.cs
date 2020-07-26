using System.Collections;
using TMPro;
using UnityEngine;
using Photon.Pun;

public class TurnsController : MonoBehaviour
{
    #region Singleton
    public static TurnsController Instance { private set; get; }
    #endregion

    [SerializeField] private float turnTime = 15f;
    private float nowTime;
    bool projectileMode = false;
    [Space]
    public TMP_Text timerText;
    public TMP_Text playerName;
    public TMP_Text unitName;

    public PhotonPlayer NowPlayer { private set; get; }
    public UnitController NowUnit
    {
        get
        {
            return NowPlayer.myUnits[unitIndex];
        }
    }

    int unitIndex = 0;
    int playerIndex = 0;

    PhotonView pv;

    void Awake()
    {
        Instance = this;
        pv = GetComponent<PhotonView>();
    }

    void Start()
    {
        nowTime = turnTime;

        NowPlayer = PhotonRoom.Instance.photonPlayers[playerIndex];
        NowPlayer.SelectUnit(NowPlayer.myUnits[unitIndex]);

        ChangeStrings();
    }

    void Update()
    {
        // Timer.
        if (projectileMode)
        {
            timerText.text = "-";
            return;
        }


        if (nowTime <= 0)
            CallNext();
        else
            nowTime -= Time.deltaTime;

        timerText.text = Mathf.Round(nowTime).ToString();
    }

    public void CallNext()
    {
        if (pv.IsMine)
            pv.RPC("Next", RpcTarget.All);
    }

    [PunRPC]
    void Next()
    {
        UIManager.Instance.ResetButtons();
        Pathfinding.Instance.gridComponent.visual.HideGrid();

        // Turn to next unit of current player.
        if (unitIndex < NowPlayer.myUnits.Count - 1)
        {
            unitIndex++;
        }
        // Turn to next player.
        else
        {
            unitIndex = 0;
            if (playerIndex < PhotonRoom.Instance.photonPlayers.Count - 1)
                playerIndex++;
            else
                playerIndex = 0;
        }

        NowPlayer = PhotonRoom.Instance.photonPlayers[playerIndex];
        NowPlayer.SelectUnit(NowPlayer.myUnits[unitIndex]);

        ChangeStrings();

        nowTime = turnTime;
    }

    public IEnumerator WaitTillProjectileDestroy(Projectile proj)
    {
        projectileMode = true;
        while (proj != null)
        {
            yield return new WaitUntil(() => proj == null);
            projectileMode = false;
            CallNext();
            yield return null;
        }
    }

    void ChangeStrings()
    {
        if (NowPlayer.pv.IsMine)
            playerName.text = "YOU";
        else
            playerName.text = NowPlayer.Owner.NickName;

        unitName.text = string.Format("{0} {1}", NowUnit.Name, (unitIndex + 1).ToString());
    }
}
