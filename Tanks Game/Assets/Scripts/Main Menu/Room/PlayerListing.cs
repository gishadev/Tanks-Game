using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerListing : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;

    public Player _PlayerInfo { get; private set; }

    public void SetPlayerListingInfo(Player player)
    {
        _PlayerInfo = player;
        _text.text = player.NickName;
    }
}
