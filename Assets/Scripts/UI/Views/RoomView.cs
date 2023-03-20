using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using System.Collections.Generic;
using System;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections;

public class RoomView : View
{
    #region BUTTON REF
    [SerializeField]
    private TMP_InputField inputRoom;
    //[SerializeField]
    //private Button rankDuelBtn;
    [SerializeField]
    private Button createRoomBtn;
    [SerializeField]
    private Button joinRoomBtn;
    [SerializeField]
    private Button logoutBtn;
    #endregion

    [SerializeField]
    private string SCENE_NAME;
    //const int rankCondition = 200;

    public RoomProfileUIBtn roomPrefabBtn;
    public Transform roomContent;
    public List<RoomInfo> updatedRooms;
    public List<RoomProfile> rooms = new List<RoomProfile>();


    public override void Initialize()
    {
        

        //Player create room
        createRoomBtn.onClick.AddListener(() =>
        {
            CreateRoom();
        });

        //Player join room
        joinRoomBtn.onClick.AddListener(() =>
        {
            JoinRoom();
        });

        ////Player đăng xuất
        logoutBtn.onClick.AddListener(() =>
        {
            PhotonNetwork.LeaveLobby();

        });


        base.Initialize();
    }

    public override void Show(object args = null)
    {
        base.Show(args);
    }

    

    

    //==============================================================

    #region METHOD BUTTONS
   

    //Convert qua Ranking Point theo từng loại máy
    public int StringToRankScore(string str)
    {
        float temp = float.Parse(str);
        Debug.Log(temp);

        if (temp > 2f)
        {
            return (int)temp;
        }
        else
        {
            return (int)(temp * 1000);
        }
    }

    public string MatchingConditionsRoom(float Rscore)
    {
        return null;
    }
    public void CreateRankingRoom(int Rscore)
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;

        float temp = Rscore / 1000f;

        PhotonNetwork.CreateRoom("ranking_" + temp.ToString("0.000") + "_" + DateTime.Now.ToOADate().ToString(), roomOptions, null);
    }

    public void CreateRoom()
    {
        if (inputRoom.text == "")
            return;
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 2;
        options.PublishUserId = true;

        PhotonNetwork.CreateRoom(inputRoom.text, options, null);
    }

    public void JoinRoom()
    {
        if (inputRoom.text == "")
            return;

        PhotonNetwork.JoinRoom(inputRoom.text);
    }

    public void Logout()
    {
        PhotonNetwork.Disconnect();
    }
    #endregion

    #region ANOTHER METHOD
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        UpdateRoomList(roomList);

    }

    public void UpdateRoomList(List<RoomInfo> roomList)
    {
        updatedRooms = roomList;

        foreach (RoomInfo roomInfo in roomList)
        {
            if (roomInfo.RemovedFromList) RoomRemove(roomInfo);
            else RoomAdd(roomInfo);
        }

        UpdateRoomProfileUI();
    }


    public void RoomAdd(RoomInfo roomInfo)
    {
        RoomProfile roomProfile = new RoomProfile();

        roomProfile = FindRoomProfileByName(roomInfo.Name);
        if (roomProfile != null) //Bị trùng lặp Room
            return;

        roomProfile = new RoomProfile
        {
            name = roomInfo.Name,
        };
        rooms.Add(roomProfile);
    }

    public void RoomRemove(RoomInfo roomInfo)
    {
        RoomProfile roomProfile = FindRoomProfileByName(roomInfo.Name);
    }
    public RoomProfile FindRoomProfileByName(string name)
    {
        foreach (RoomProfile roomProfile in rooms)
        {
            if (roomProfile.name == name)
                return roomProfile;
        }

        return null;
    }

    public void UpdateRoomProfileUI()
    {
        foreach (Transform child in roomContent)
        {
            Destroy(child.gameObject);
        }

        foreach (RoomProfile roomProfile in rooms)
        {
            RoomProfileUIBtn roomProfileUIBtn = Instantiate(roomPrefabBtn);
            roomProfileUIBtn.SetRoomProfile(roomProfile);
            roomProfileUIBtn.transform.SetParent(roomContent);

        }
    }
    #endregion
}
