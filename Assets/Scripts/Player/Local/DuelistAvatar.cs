using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DuelistAvatar : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI playerNameTxt;
    [SerializeField]
    private TextMeshProUGUI opponentNameTxt;


    private void Update()
    {
        if (PhotonDuelistStats.Instance == null)
            return;

        if (!Field_Manager_Id.Instance.isDuelStart)
            return;

        UpdateDuelistUI();
    }

    public void UpdateDuelistUI()
    {
        foreach (var duelist in PhotonDuelistStats.Instance.duelists)
        {
            if (duelist == null)
                return;

            if (duelist.GetUserName() == PhotonNetwork.LocalPlayer.NickName.ToString())
            {
                playerNameTxt.text = duelist.GetUserName();
            }
            else
            {
                opponentNameTxt.text = duelist.GetUserName();
            }
        }
    }
}
