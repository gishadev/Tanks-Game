using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class RoomListing : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TMP_Text _text;

    public RoomInfo _RoomInfo { get; private set; }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            PhotonMaster.Instance.JoinRoom(_RoomInfo.Name);
        }
    }

    public void SetRoomListingInfo(RoomInfo roomInfo)
    {
        //Saving room info
        _RoomInfo = roomInfo;
        _text.text = string.Format("[{0}/{1}]  {2}", roomInfo.PlayerCount, roomInfo.MaxPlayers, roomInfo.Name);
    }
}
