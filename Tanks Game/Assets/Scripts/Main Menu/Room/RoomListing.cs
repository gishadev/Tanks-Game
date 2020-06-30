using Photon.Realtime;
using TMPro;
using UnityEngine;

public class RoomListing : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;

    public RoomInfo _RoomInfo { get; private set; }

    public void SetRoomInfo(RoomInfo roomInfo)
    {
        //Saving room info
        _RoomInfo = roomInfo;
        _text.text = string.Format("[{0}/{1}]  {2}", roomInfo.MaxPlayers, roomInfo.PlayerCount, roomInfo.Name);
    }
}
