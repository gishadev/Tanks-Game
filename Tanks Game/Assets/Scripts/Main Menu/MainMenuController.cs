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

    void Awake()
    {
        Instance = this;    
    }

    public void onClick_OpenMenu(GameObject toOpen)
    {
        toOpen.SetActive(true);
        lobbyMenu.SetActive(false);
    }
    public void onClick_ReturnToLobby(GameObject toClose)
    {
        lobbyMenu.SetActive(true);
        toClose.SetActive(false);
    }
}
