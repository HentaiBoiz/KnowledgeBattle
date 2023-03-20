using System;
using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;

[Serializable]
public class PlayerProfile
{

    #region PLAYFAB VARIABLE
    private string userId; //PlayfabId
    private string userName; //Username Playfab = Photon
    private string currSessionTicket;
    
    #endregion


    //Other
    public int goFirst = 0; //0: Chưa ai chọn gì, 1: Chọn đi trước, 2: Chọn đi sau

    public string[] deckCardIds = null;

    #region GETTER SETTER
    public void SetUserId(string id)
    {
        userId = id;
    }
    public string GetUserId()
    {
        return userId;
    }

    public void SetUserName(string name)
    {
        userName = name;
    }
    public string GetUserName()
    {
        return userName;
    }

    public void SetSessionTicket(string sstk)
    {
        currSessionTicket = sstk;
    }
    public string GetSessionTicket()
    {
        return currSessionTicket;
    }
    #endregion

}
