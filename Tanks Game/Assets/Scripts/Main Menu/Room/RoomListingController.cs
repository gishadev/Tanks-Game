using Photon.Realtime;
using Photon.Pun;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
public class RoomListingController : MonoBehaviourPunCallbacks
{
    #region Creating
    public Transform contentTrans;
    public GameObject roomListingPrefab;
    public TMP_Text roomNameInput;
    #endregion

    private List<RoomListing> listings = new List<RoomListing>();

    public void OnClick_CreateRoom()
    {
        PhotonMaster.Instance.CreateRoom(roomNameInput.text);
        Debug.Log("Creating room.");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {
            int index = listings.FindIndex(x => x._RoomInfo.Name == info.Name);
            // Removed room listing.
            if (info.RemovedFromList)
            {
                if (index != -1)
                {
                    Destroy(listings[index].gameObject);
                    listings.RemoveAt(index);
                }
            }
            else
            {
                // Adding a new room listing.
                if (index == -1)
                {
                    GameObject listingGO = Instantiate(roomListingPrefab, contentTrans);
                    RoomListing listing = listingGO.GetComponent<RoomListing>();

                    if (listing != null)
                        listing.SetRoomListingInfo(info);

                    listings.Add(listing);
                }

                // Modifying room listing.
                else
                {
                    // Update players count.
                    listings[index].SetRoomListingInfo(info);
                }
            }
        }
    }
}
