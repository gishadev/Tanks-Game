using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class PhotonLobby : MonoBehaviourPunCallbacks
{
    #region Singleton
    public static PhotonLobby Instance;
    #endregion

    public GameObject startFastPlayBtn;
    public GameObject cancelFastPlayBtn;

    public TMP_Text lastLog;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        CreateLog("Successfully connected to master.");

        startFastPlayBtn.SetActive(true);
    }

    public void OnFastPlayBtn()
    {
        CreateLog("Trying to join a random room...");

        startFastPlayBtn.SetActive(false);
        cancelFastPlayBtn.SetActive(true);

        PhotonNetwork.JoinRandomRoom();
    }

    public void OnCancelFastPlayBtn()
    {
        CreateLog("Canceled room searching.");

        startFastPlayBtn.SetActive(true);
        cancelFastPlayBtn.SetActive(false);

        PhotonNetwork.LeaveRoom();
    }

    public override void OnJoinedRoom()
    {
        CreateLog(string.Format("Successfully joined to a room {0}", PhotonNetwork.CurrentRoom.Name));

        startFastPlayBtn.SetActive(false);
        cancelFastPlayBtn.SetActive(true);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        CreateLog("Failed to join a random room...");

        CreateRoom();
    }

    void CreateRoom()
    {
        CreateLog("Creating Room");

        int roomName = Random.Range(0, 10000);
        RoomOptions roomOptions = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = 10 };
        PhotonNetwork.CreateRoom(roomName.ToString(), roomOptions);
    }

    public override void OnCreatedRoom()
    {
        CreateLog(string.Format("Successfully created a room", PhotonNetwork.CurrentRoom.Name));

        startFastPlayBtn.SetActive(false);
        cancelFastPlayBtn.SetActive(true);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        CreateLog("Failed to create room. Maybe there is another room with the same name.");
        CreateRoom(); // Trying to create new room.
    }

    void CreateLog(string text)
    {
        Debug.Log(text);
        lastLog.text = text;
    }
}
