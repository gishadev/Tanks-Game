using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerListingController : MonoBehaviourPunCallbacks
{
    #region Creating
    public Transform contentTrans;
    public GameObject playerListingPrefab;
    #endregion

    private List<PlayerListing> listings = new List<PlayerListing>();

    public void CreatePlayersListings()
    {
        ClearOldListings();
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            GameObject playerListingGO = Instantiate(playerListingPrefab, contentTrans);
            PlayerListing listing = playerListingGO.GetComponent<PlayerListing>();

            if (listing != null)
                listing.SetPlayerListingInfo(p);

            listings.Add(listing);
        }
    }

    void ClearOldListings()
    {
        foreach(PlayerListing l in listings)
        {
            Destroy(l.gameObject);
        }
        listings.Clear();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        GameObject playerListingGO = Instantiate(playerListingPrefab, contentTrans);
        PlayerListing listing = playerListingGO.GetComponent<PlayerListing>();

        if (listing != null)
            listing.SetPlayerListingInfo(newPlayer);

        listings.Add(listing);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        int index = listings.FindIndex(x => x._PlayerInfo == otherPlayer);

        if (index != -1)
        {
            Destroy(listings[index].gameObject);
            listings.RemoveAt(index);
        }
    }
}
