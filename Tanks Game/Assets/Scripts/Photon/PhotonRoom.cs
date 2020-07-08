using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhotonRoom : MonoBehaviourPunCallbacks
{
    #region Singleton
    public static PhotonRoom Instance { private set; get; }
    #endregion

    private int gameSceneIndex = 1;
    private int lobbySceneIndex = 0;

    public List<PhotonPlayer> photonPlayers = new List<PhotonPlayer>();

    void Awake()
    {
        // Setting up singleton.
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
        DontDestroyOnLoad(this.gameObject);
    }

    public override void OnEnable()
    {
        base.OnEnable();
        PhotonNetwork.AddCallbackTarget(this);
        SceneManager.sceneLoaded += OnSceneFinishedLoading;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        PhotonNetwork.RemoveCallbackTarget(this);
        SceneManager.sceneLoaded -= OnSceneFinishedLoading;
    }

    void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == gameSceneIndex)
            CreatePlayer();
    }

    void CreatePlayer()
    {
        PhotonNetwork.Instantiate(Path.Combine("Prefabs", "Photon", "Photon Player"), Vector3.zero, Quaternion.identity);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        int index = photonPlayers.FindIndex(x => x.Id == otherPlayer.ActorNumber);

        if (index != -1)
            photonPlayers.RemoveAt(index);

        Debug.Log(photonPlayers.Count);
    }
}
