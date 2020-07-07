using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager Instance { private set; get; }
    #endregion
    public CameraTransform cam;

    public GameObject playerPrefab;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
}
