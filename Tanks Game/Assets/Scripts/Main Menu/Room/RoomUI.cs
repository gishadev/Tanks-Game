using Photon.Realtime;
using Photon.Pun;
using UnityEngine;
using TMPro;

public class RoomUI : MonoBehaviour
{
    public RoomInfo _RoomInfo { private set; get; }
    public TMP_Text nameText;


    public void SetRoomInfo(RoomInfo roomInfo)
    {
        _RoomInfo = roomInfo;

        nameText.text = roomInfo.Name;
    }

    public void Show(RoomInfo roomInfo)
    {
        SetRoomInfo(roomInfo);

        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void OnClick_LeaveRoom()
    {
        PhotonMaster.Instance.LeaveRoom();
        Hide();
    }
}
