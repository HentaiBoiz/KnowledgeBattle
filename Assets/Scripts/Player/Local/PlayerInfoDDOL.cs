using Photon.Pun;
using PlayFab.ClientModels;
using UnityEngine;

public class PlayerInfoDDOL : MonoBehaviour
{
    public static PlayerInfoDDOL Instance;

    public PlayerProfile playerProfile; //Lưu các biến của Playfab, Photon

    //Position
    public Vector3 lastPosition; //Position cuối cùng của Player


    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        Instance = this;
    }


    #region PLAYFAB METHOD

    #endregion

    #region NORMAL METHOD
    public void SetAccountInfo(string id, string name, string ticket)
    {
        playerProfile.SetUserId(id);
        playerProfile.SetUserName(name);
        playerProfile.SetSessionTicket(ticket);
    }

    public void SetPlayerLastPos(Vector3 position)
    {
        lastPosition = position;
    }

    public Vector3 GetPlayerLastPos()
    {
        return lastPosition;
    }
    #endregion
}
