using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PhotonMaster : MonoBehaviourPunCallbacks
{
    #region Singleton
    public static PhotonMaster Instance { get; private set; }
    #endregion

    public ClientData nowClient { get; private set; }

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            if (Instance != this)
            {
                Destroy(Instance.gameObject);
                Instance = this;
            }
        }
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        nowClient = GetComponent<ClientData>();

        Debug.Log("Connecting to Photon...");

        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Successfully connected to Photon.");

        JoinLobby();
    }

    public void JoinLobby()
    {
        Debug.Log("Joining a lobby.");

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            MainMenuController.Instance.ChangeMenuTo(MainMenuController.Instance.lobbyMenu);
            MainMenuController.Instance.loadingScreen.SetActive(true);

            PhotonNetwork.JoinLobby();
        }
        else
            SceneManager.LoadScene(0);
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Successfully joined to a lobby.");

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
        Debug.Log("Successfully created a room.");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to create a room. Maybe there is another room with the same name.");
    }

    public void JoinRoom(string _name)
    {
        Debug.Log("Joining room.");
        PhotonNetwork.JoinRoom(_name);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Successfully joined a room.");
        RoomReferences.Instance._RoomUI.Show(PhotonNetwork.CurrentRoom);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to join a room " + message + ".");
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Disconnected.");
        JoinLobby();
    }
}
