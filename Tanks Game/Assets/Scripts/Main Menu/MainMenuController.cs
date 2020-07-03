using UnityEngine;
using TMPro;

public class MainMenuController : MonoBehaviour
{
    #region Singleton
    public static MainMenuController Instance;
    #endregion

    public TMP_Text lastLog;
    [Space]
    public GameObject[] menus;
    public GameObject lobbyMenu;
    void Awake()
    {
        Instance = this;
    }

    public void ChangeMenuTo(GameObject target)
    {
        foreach (GameObject menu in menus)
            menu.SetActive(false);
        target.SetActive(true);
    }
}
