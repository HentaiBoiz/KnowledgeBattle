using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

public class Photon_Room_Manager : MonoBehaviourPunCallbacks
{
    public static Photon_Room_Manager Instance;


    [SerializeField]
    private string SCENE_NAME;

    //Room Update
    public List<RoomInfo> updatedRooms;
    public List<RoomProfile> rooms = new List<RoomProfile>();
    public CustomBattleView customBattleView;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        PhotonNetwork.JoinLobby();
    }

    #region PHOTON CALLBACKS
    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel(SCENE_NAME);
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby");
    }

    public override void OnLeftLobby()
    {
        PhotonNetwork.LoadLevel("1_OfflineScene");
    }
    #endregion

    #region UPDATE ROOM
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

        //Nếu đang ở View CustomBattle thì cập nhật UI
        if(customBattleView.gameObject.activeSelf == true)
        {
            customBattleView.UpdateRoomProfileUI();
        }
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

    public void SetCustomInputField(string txt)
    {
        //Nếu đang ở View CustomBattle thì cập nhật UI
        if (customBattleView.gameObject.activeSelf == true)
        {
            customBattleView.inputRoom.text = txt;
        }
    }
    #endregion
}
