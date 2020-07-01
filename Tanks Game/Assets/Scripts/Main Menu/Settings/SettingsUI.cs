using TMPro;
using UnityEngine;

public class SettingsUI : MonoBehaviour
{
    public TMP_Text nicknameText;
    public TMP_Text oldNicknameText;

    void Start()
    {
        UpdateCurrentNicknameUI();
    }

    public void onClick_Apply()
    {
        PhotonMaster.Instance.nowClient.SetNickname(nicknameText.text);
        UpdateCurrentNicknameUI();
    }

    void UpdateCurrentNicknameUI()
    {
        oldNicknameText.text = PhotonMaster.Instance.nowClient.Nickname;
    }
}
