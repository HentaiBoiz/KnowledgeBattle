using ExitGames.Client.Photon;
using Newtonsoft.Json;
using Photon.Pun;
using Photon.Realtime;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PhotonDuelistStats : MonoBehaviourPunCallbacks
{

    public static PhotonDuelistStats Instance;

    public PlayerProfile[] duelists = new PlayerProfile[2];

    //Check All Ready ?
    public bool[] playerReadys = new bool[2];

    public DeckJson deckJson = null;

    PhotonView _photonView;

    private void Awake()
    {
        Instance = this;

        //Đưa tất cả biến ready về false
        for (int i = 0; i < playerReadys.Count(); i++)
        {
            playerReadys[i] = false;
        }
    }

    private void Start()
    {
        _photonView = GetComponent<PhotonView>();
 
    }


    public void LoadRoomDuelists()
    {
        LoadDeckFromPlayfab();
    }

    #region RPC METHOD
    public void RPC_UpdateRoomDuelist(PlayerProfile duelist, int index)
    {
        photonView.RPC("UpdateRoomDuelist", RpcTarget.All,duelist.GetUserId(), duelist.GetUserName(), deckJson.cardIds.ToArray(), duelist.goFirst, index);
    }
    [PunRPC]
    private void UpdateRoomDuelist(string id, string nickname, string[] deckids, int goFirst, int index)
    {
        PlayerProfile playerProfile;

        playerProfile = new PlayerProfile
        {
            goFirst = goFirst,
            deckCardIds = deckids
        };
        playerProfile.SetUserId(id);
        playerProfile.SetUserName(nickname);

        duelists[index] = playerProfile;

        StartLoadCardImg();

        //Set Player ready sau 2s
        Invoke("SetPlayerReady", 2f);
    }

    public void SetPlayerReady()
    {
        photonView.RPC("RPC_SetPlayerReady", RpcTarget.All, TurnManager.Instance.localSide);
    }
    [PunRPC]
    public void RPC_SetPlayerReady(int side)
    {
        playerReadys[side] = true;
    }
    public bool AllIsReady()
    {
        for (int i = 0; i < playerReadys.Count(); i++)
        {
            if (playerReadys[i] == false)
                return false;
        }

        return true;
    }
    #endregion

    #region RAISED EVENT METHOD
    public void StartLoadCardImg()
    {
        object[] datas = new object[] { }; //Đóng gói

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };

        PhotonNetwork.RaiseEvent(
            ((byte)EventCode.onLoadCardImg),
            datas,
            raiseEventOptions,
            SendOptions.SendReliable);
    }
    #endregion

    #region PLAYFAB METHOD
    public void LoadDeckFromPlayfab()
    {
        List<DeckJson> deckJsons = new List<DeckJson>();

        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), result =>
        {
            if (result.Data != null && result.Data.ContainsKey("Decks"))
            {
                deckJsons = JsonConvert.DeserializeObject<List<DeckJson>>(result.Data["Decks"].Value);
                Debug.Log("LOAD DECK SUCCESS");
            }
            else
            {

                Debug.LogError("NO DECK WAS FOUNDED ! DUEL WILL BE CANCEL");
            }

            deckJson = new DeckJson(deckJsons[PlayfabUserInfomation.Instance.playerData.currDeckSlot]);
            deckJson.cardIds = deckJson.cardIds.OrderBy(i => Guid.NewGuid()).ToList(); //Random


            //Photon RPC
            if (PhotonNetwork.IsMasterClient)
            {
                RPC_UpdateRoomDuelist(PlayerInfoDDOL.Instance.playerProfile, 0);
            }
            else
            {
                RPC_UpdateRoomDuelist(PlayerInfoDDOL.Instance.playerProfile, 1);
            }

        },
            error =>
            {
                Debug.Log(error.GenerateErrorReport());
            });
    }
    #endregion
}
