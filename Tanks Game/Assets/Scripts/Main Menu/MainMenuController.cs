using UnityEngine;
using TMPro; 


public class MainMenuController : MonoBehaviour
{
    #region Singleton
    public static MainMenuController Instance;
    #endregion

    public TMP_Text lastLog;
    [Space]
    public GameObject lobbyMenu;
    public GameObject roomMenu;
    public GameObject roomsListingMenu;

    void Awake()
    {
        Instance = this;    
    }

    public void OpenRoomsListing()
    {
        lobbyMenu.SetActive(false);
        roomMenu.SetActive(false);
        roomsListingMenu.SetActive(true);
    }

    public void OpenRoomMenu()
    {
        lobbyMenu.SetActive(false);
        roomMenu.SetActive(true);
        roomsListingMenu.SetActive(false);
    }

    public void OpenLobbyMenu()
    {
        lobbyMenu.SetActive(true);
        roomMenu.SetActive(false);
        roomsListingMenu.SetActive(false);
    }
}
