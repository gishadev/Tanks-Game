using Photon.Realtime;
using Photon.Pun;
using UnityEngine;

public class ClientData : MonoBehaviour
{
    public string Nickname { private set; get; }

    void Awake()
    {
        string randomName = "Player" + Random.Range(0, 100000);
        SetNickname(randomName);
    }

    public void SetNickname(string _text)
    {
        Nickname = _text;
        PhotonNetwork.LocalPlayer.NickName = _text;
    }
}
