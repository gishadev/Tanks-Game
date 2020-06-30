using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PhotonMaster : MonoBehaviourPunCallbacks
{
    #region Singleton
    public static PhotonMaster Instance { get; private set; }
    #endregion

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Debugger.CreateLog("Connecting to Photon...");

        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debugger.CreateLog("Successfully connected to Photon.");

        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debugger.CreateLog("Successfully joined to a lobby.");
    }

    public void CreateRoom(string _name)
    {
        RoomOptions opt = new RoomOptions() { MaxPlayers = 10, IsOpen = true, IsVisible = true };
        PhotonNetwork.JoinOrCreateRoom(_name, opt, TypedLobby.Default);
    }

    public override void OnCreatedRoom()
    {
        Debugger.CreateLog("Successfully created a room.");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debugger.CreateLog("Failed to create a room. Maybe there is another room with the same name.");
    }
}
