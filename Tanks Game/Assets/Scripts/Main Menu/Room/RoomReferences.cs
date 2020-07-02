using UnityEngine;

public class RoomReferences : MonoBehaviour
{
    #region Singleton
    public static RoomReferences Instance { private set; get; }
    #endregion

    public RoomListingController _RoomListingController;
    public PlayerListingController _PlayerListingController;
    public RoomUI _RoomUI;

    void Awake()
    {
        Instance = this;
    }
}
