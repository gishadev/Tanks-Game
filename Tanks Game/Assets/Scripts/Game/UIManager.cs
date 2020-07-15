using UnityEngine;

public class UIManager : MonoBehaviour
{
    #region Singleton
    public static UIManager Instance;
    #endregion

    public GameObject shoot_Btn;
    public GameObject cancelShoot_Btn;

    void Awake()
    {
        Instance = this;
    }

    public void ShowShootBtn()
    {
        shoot_Btn.SetActive(true);
        cancelShoot_Btn.SetActive(false);
    }
    public void HideShootBtn()
    {
        shoot_Btn.SetActive(false);
        cancelShoot_Btn.SetActive(true);
    }
}
