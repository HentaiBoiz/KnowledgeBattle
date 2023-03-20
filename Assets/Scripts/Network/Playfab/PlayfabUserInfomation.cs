using Newtonsoft.Json;
using Photon.Pun;
using Photon.Realtime;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[Serializable]
public class UserLoginTime
{
    public string ssTicket;
    public string dateTime;

    public UserLoginTime()
    {

    }

    public UserLoginTime(string ssTicket, string dateTime)
    {
        this.ssTicket = ssTicket;
        this.dateTime = dateTime;
    }
}

[Serializable]
public class PlayerData
{
    public int currDeckSlot; //Slot Deck mà người chơi đang sử dụng
    public int punish = 0; //Bị phạt bao nhiêu điểm Rank

    public PlayerData()
    {
        
    }

    public PlayerData(int currDeckSlot)
    {
        this.currDeckSlot = currDeckSlot;
    }
}

public class PlayfabUserInfomation : MonoBehaviourPunCallbacks

{

    public enum AccountStats
    {
        ReadyLogin,
        CheckingLogin,
        LoginCompleted,
        LoginFailed,
        ReadyToPlay
    }

    public static PlayfabUserInfomation Instance;

    //Các state của tài khoản này
    public AccountStats accountStats;

    #region PLAYER DATA
    private string curPlayfabID;
    private string curSessionTicket;

    public PlayerData playerData = null;
    #endregion

    #region LOGIN TIME CHECK
    public UserLoginTime userLoginTime = null;
    public UserLoginTime loadedLoginTime = null;
    
    private float updateTime = 60f; //thời gian update lại LoginTime 1p
    private float countDownUpdateTime = 0f;
    bool getLoadLoginTime = false; //Đã load chưa
    const double _1minute = 0.00070563079;
    #endregion


    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        Instance = this;
    }

    private void Start()
    {
        accountStats = AccountStats.ReadyLogin;
    }

    private void Update()
    {
        countDownUpdateTime -= Time.deltaTime;

        switch (accountStats)
        {
            case AccountStats.ReadyLogin:
                break;
            case AccountStats.CheckingLogin:
                CheckDuplicateLogin();
                break;
            case AccountStats.LoginCompleted:
                OnLoginCompleted();
                break;
            case AccountStats.LoginFailed:
                OnLoginFailed();
                break;
            case AccountStats.ReadyToPlay:
                if(countDownUpdateTime <= 0)
                {
                    UpdatePlayerLoginTime();
                    countDownUpdateTime = updateTime;
                }
                break;
        }
    }



    #region LOGIN METHOD
    public void SetUserData(string playfabId, string sessionTicket)
    {
        curPlayfabID = playfabId;
        curSessionTicket = sessionTicket;

        accountStats = AccountStats.CheckingLogin;
    }
    //Khi đăng nhập thành công thì sẽ chạy hàm này
    private void OnLoginCompleted()
    {
        //Chuyển state của Player sang Ready To Play
        accountStats = AccountStats.ReadyToPlay;

        //Lấy dữ liệu Account
        GetAccountInfoRequest request = new GetAccountInfoRequest ();
        PlayFabClientAPI.GetAccountInfo(request, result =>
        {
            //Login thành công thì kết nối vào photon
            PhotonNetwork.LocalPlayer.NickName = result.AccountInfo.TitleInfo.DisplayName; //Set Photon Nickname
           
            PlayerInfoDDOL.Instance.playerProfile.SetUserName(result.AccountInfo.TitleInfo.DisplayName); //Set Displayname
            PlayfabCardDB.Instance.LoadAllContractFromPlayfab(); //Load Contract Address từ server
            PlayfabCardDB.Instance.LoadAllCardFromPlayfab(); //Load Item Card từ server

            //Load dữ liệu Player
            LoadPlayerData();

            UpdatePlayerLoginTime();
            PhotonNetwork.ConnectUsingSettings();

        }, error =>
        {
            Debug.Log(error.GenerateErrorReport());
            ErrorsManager.Instance.PushError(error.GenerateErrorReport());
        });

    }
    private void OnLoginFailed()
    {
        accountStats = AccountStats.ReadyLogin;
        getLoadLoginTime = false;
    }

    //public override void OnDisconnected(DisconnectCause cause)
    //{
    //    base.OnDisconnected(cause);
    //    LogoutPlayerLoginTime();
    //    Debug.Log("Disconnect");
    //}

    public void OnApplicationQuit()
    {
        LogoutPlayerLoginTime();
        Debug.Log("Disconnect");

    }

    #endregion

    #region PLAYFAB METHOD
    //Nếu Player vẫn còn connect thì cứ mỗi 2p sẽ update 1 lần
    public void UpdatePlayerLoginTime()
    {
        userLoginTime = new UserLoginTime(curSessionTicket, DateTime.Now.ToOADate().ToString().Replace(",", "."));
        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                { "LoginTime", JsonConvert.SerializeObject(userLoginTime) }
            },

            Permission = UserDataPermission.Private

        };
        PlayFabClientAPI.UpdateUserData(request, result =>
        {

            Debug.Log("UPLOAD LOGINTIME SUCCESS");
        },
        error =>
        {
            Debug.Log(error.GenerateErrorReport());
            ErrorsManager.Instance.PushError(error.GenerateErrorReport());
        });
    }

    public void LogoutPlayerLoginTime()
    {
        userLoginTime = new UserLoginTime(curSessionTicket, (DateTime.Now.ToOADate() - _1minute*2).ToString().Replace(",", "."));
        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                { "LoginTime", JsonConvert.SerializeObject(userLoginTime) }
            },

            Permission = UserDataPermission.Private

        };
        PlayFabClientAPI.UpdateUserData(request, result =>
        {

            Debug.Log("UPLOAD LOGINTIME SUCCESS");
        },
        error =>
        {
            Debug.Log(error.GenerateErrorReport());
            ErrorsManager.Instance.PushError(error.GenerateErrorReport());
        });
    }
    //Kiểm tra xem có máy nào đang đăng nhập trước đó rồi hay không, nếu có rồi thì ngắt kết nối máy này
    private void CheckDuplicateLogin()
    {
        if (getLoadLoginTime == false) //Chưa load thì mới load
            GetLoginTime();
        //Đợi load xong
        if (userLoginTime == null || loadedLoginTime == null)
            return;

        try
        {
            //Bắt đầu Check
            if (double.Parse(userLoginTime.dateTime) - double.Parse(loadedLoginTime.dateTime) < _1minute)
            {
                Debug.LogError("This account is being used by another computer ! LOGIN FAILED");
                accountStats = AccountStats.LoginFailed;
            }
            else
            {
                Debug.Log("LOGIN SUCCESSFULL !");
                accountStats = AccountStats.LoginCompleted;
            }
        }
        catch (Exception)
        {
        }
        
    }

    //Debug lấy Minute
    IEnumerator TakeTime()
    {
        accountStats = AccountStats.CheckingLogin;
        DateTime d1, d2;
        d1 = DateTime.Now;
        yield return new WaitForSeconds(120f);
        d2 = DateTime.Now;

        //0.00141126159 - 2 minute
        Debug.LogError(d2.ToString() + "->" + d2.ToOADate() + " - " + d1.ToString() + "->" + d1.ToOADate());
    }

    private void GetLoginTime()
    {
        getLoadLoginTime = true;
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), result =>
        {
            if (result.Data != null && result.Data.ContainsKey("LoginTime"))
            {
                loadedLoginTime = JsonConvert.DeserializeObject<UserLoginTime>(result.Data["LoginTime"].Value);
                userLoginTime = new UserLoginTime(curSessionTicket, DateTime.Now.ToOADate().ToString().Replace(",","."));
                Debug.Log("LOAD LOGIN TIME SUCCESS");

            }
            else
            {
                //OnLoginCompleted()
                loadedLoginTime = new UserLoginTime(curSessionTicket, (DateTime.Now.ToOADate() - _1minute).ToString().Replace(",", "."));
                userLoginTime = new UserLoginTime(curSessionTicket, DateTime.Now.ToOADate().ToString().Replace(",", ".")); 
                Debug.Log("NO LOGIN WAS FOUNDED ! YOU CAN ACCESS THE GAME NOW");
            }

        },
        error =>
        {
            Debug.Log(error.GenerateErrorReport());
            ErrorsManager.Instance.PushError(error.GenerateErrorReport());
        });
    }

    public void LoadPlayerData() //Load dữ liệu PlayerData từ Playfab về
    {
        ErrorsManager.Instance.LoadListErrorFromPlayfab(); //load list loi ve 

        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), result =>
        {
            if (result.Data != null && result.Data.ContainsKey("PlayerData"))
            {
                playerData = JsonConvert.DeserializeObject<PlayerData>(result.Data["PlayerData"].Value);
                

                Debug.Log("LOAD PLAYER DATA SUCCESS - Punish: " + playerData.punish);

            }
            else
            {
                playerData = new PlayerData(0); //Khởi lại Player Data mới

                Debug.Log("NO PLAYER DATA WAS FOUNDED ! CREATE A NEW ONE");
            }

        },
        error =>
        {
            Debug.Log(error.GenerateErrorReport());
            ErrorsManager.Instance.PushError(error.GenerateErrorReport());
        });
    }

    public void UploadPlayerData()
    {
        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                { "PlayerData", JsonConvert.SerializeObject(playerData) }
            },

            Permission = UserDataPermission.Public

        };
        PlayFabClientAPI.UpdateUserData(request, result =>
        {

            Debug.Log("UPDATE PLAYER DATA SUCCESS");
        },
        error =>
        {
            Debug.Log(error.GenerateErrorReport());
            ErrorsManager.Instance.PushError(error.GenerateErrorReport());
        });
    }

    //Khi trận đấu mới bắt đầu thì set hình phạt
    public void SetPlayerPunish(int punish)
    {
        playerData.punish = punish;
        UploadPlayerData();
    }

    //Trừ điểm Rank theo Punish
    public void SubVC(int punish)
    {

        SubtractUserVirtualCurrencyRequest virtualCurrencyRequest = new SubtractUserVirtualCurrencyRequest
        {
            Amount = punish,
            VirtualCurrency = "RS",

        };
        PlayFabClientAPI.SubtractUserVirtualCurrency(virtualCurrencyRequest, SubVCComplete, OnPlayfabError);

    }

    public void SubVCComplete(ModifyUserVirtualCurrencyResult VC)
    {
        Debug.LogWarning($"ABANDONED GAME PENATLY: You received a Rank Score penalty ({playerData.punish}) for leaving a recent game ! We expect player to try their hardest in each game they play !");

        SetPlayerPunish(0);

    }
    public void OnPlayfabError(PlayFabError error)
    {
        string textError = error.GenerateErrorReport();

        ErrorsManager.Instance.PushError(textError);
        Debug.LogWarning(textError);

    }
    #endregion


}
