using Photon.Realtime;
using Photon.Pun;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
public class RoomCreator : MonoBehaviourPunCallbacks
{
    public Transform contentTrans;
    public GameObject roomUIElementPrefab;

    public TMP_Text roomNameInput;

    private List<RoomListing> listings = new List<RoomListing>();

    public void OnClick_CreateRoom()
    {
        PhotonMaster.Instance.CreateRoom(roomNameInput.text);
        Debugger.CreateLog("Creating room.");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach(RoomInfo info in roomList)
        {
            // Removed from rooms list.
            if (info.RemovedFromList)
            {
                int i = listings.FindIndex(x => x._RoomInfo.Name == info.Name);

                if (i != -1)
                {
                    Destroy(listings[i].gameObject);
                    listings.RemoveAt(i);
                }
            }

            // Added to rooms list.
            else
            {
                GameObject listingGO = Instantiate(roomUIElementPrefab, contentTrans);
                RoomListing listing = listingGO.GetComponent<RoomListing>();

                if (listing != null)
                    listing.SetRoomInfo(info);
            }
        }
    }
}
