using Photon.Realtime;
using Photon.Pun;
using UnityEngine;
using TMPro;

public class RoomUI : MonoBehaviour
{
    public GameObject roomUIGO;
    public GameObject startGameBtn;

    public RoomInfo _RoomInfo { private set; get; }

    public TMP_Text roomName;
    public TMP_Text playersCount;


    public void SetRoomInfo(RoomInfo roomInfo)
    {
        _RoomInfo = roomInfo;
        roomName.text = roomInfo.Name;
        SetPlayersCount();

        // If current client is host => able to start a game.
        if (PhotonNetwork.IsMasterClient)
            startGameBtn.SetActive(true);
        else
            startGameBtn.SetActive(false);

        // Adding all players listings.
        RoomReferences.Instance._PlayerListingController.CreateNewPlayersListings();
    }

    public void SetPlayersCount()
    {
        playersCount.text = string.Format("[{0}/{1}]", PhotonNetwork.CurrentRoom.PlayerCount, PhotonNetwork.CurrentRoom.MaxPlayers);
    }

    public void Show(RoomInfo roomInfo)
    {
        SetRoomInfo(roomInfo);

        roomUIGO.SetActive(true);
    }

    public void OnClick_LeaveRoom()
    {
        PhotonMaster.Instance.LeaveRoom();
    }

    public void onClick_StartGame()
    {
        PhotonNetwork.LoadLevel(1);
    }
}
