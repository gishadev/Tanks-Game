using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PhotonMaster : MonoBehaviourPunCallbacks
{
    #region Singleton
    public static PhotonMaster Instance { get; private set; }
    #endregion

    public ClientData nowClient { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        nowClient = GetComponent<ClientData>();

        Debugger.CreateLog("Connecting to Photon...");

        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debugger.CreateLog("Successfully connected to Photon.");

        JoinLobby();
    }

    public void JoinLobby()
    {
        Debugger.CreateLog("Joining a lobby.");

        MainMenuController.Instance.ChangeMenuTo(MainMenuController.Instance.lobbyMenu);
        MainMenuController.Instance.loadingScreen.SetActive(true);

        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debugger.CreateLog("Successfully joined to a lobby.");

        MainMenuController.Instance.loadingScreen.SetActive(false);
    }

    public void CreateRoom(string _name)
    {
        // Setting room name.
        string name;
        if (_name.Length <= 1)
            name = string.Format("{0}'s Room", nowClient.Nickname);
        else
            name = _name;

        // Setting room options.
        RoomOptions opt = new RoomOptions() { MaxPlayers = 10, IsOpen = true, IsVisible = true };
        PhotonNetwork.JoinOrCreateRoom(name, opt, TypedLobby.Default);
    }

    public override void OnCreatedRoom()
    {
        Debugger.CreateLog("Successfully created a room.");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debugger.CreateLog("Failed to create a room. Maybe there is another room with the same name.");
    }

    public void JoinRoom(string _name)
    {
        Debugger.CreateLog("Joining room.");
        PhotonNetwork.JoinRoom(_name);
    }

    public override void OnJoinedRoom()
    {
        Debugger.CreateLog("Successfully joined a room.");
        RoomReferences.Instance._RoomUI.Show(PhotonNetwork.CurrentRoom);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debugger.CreateLog("Failed to join a room " + message + ".");
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debugger.CreateLog("Disconnected.");
        JoinLobby();
    }
}
