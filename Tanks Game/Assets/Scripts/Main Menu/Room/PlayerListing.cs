using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class PlayerListing : MonoBehaviour
{
    [SerializeField] private TMP_Text nickname;

    [SerializeField] private GameObject kickBtn;
    [SerializeField] private GameObject outlineImg;
    [SerializeField] private GameObject hostImg;

    public Player _PlayerInfo { get; private set; }

    public void SetPlayerListingInfo(Player player)
    {
        _PlayerInfo = player;
        nickname.text = player.NickName;

        // Checking if current player is local.
        if (player.IsLocal) outlineImg.SetActive(true);
        else outlineImg.SetActive(false);

        // Checking if current player is host.
        if (player.IsMasterClient) hostImg.SetActive(true);
        else hostImg.SetActive(false);

        // If local player is host => adding kick btn to other players

        //Only local host entry
        if (!PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            kickBtn.SetActive(false);
            return;
        }
        if (PhotonNetwork.LocalPlayer == player)
            kickBtn.SetActive(false);
        else
            kickBtn.SetActive(true);
    }

    public void onClick_Kick()
    {
        // Disconnecting the chosen player.
        PhotonNetwork.CloseConnection(_PlayerInfo);
    }
}
