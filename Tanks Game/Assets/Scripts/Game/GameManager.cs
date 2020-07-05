using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager Instance { private set; get; }
    #endregion
    public CameraTransform cam;

    public GameObject playerPrefab;

    public Transform[] spawnpoints;

    private void OnEnable()
    {
        if (Instance == null)
            Instance = this;
    }
}
