using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CustomBattleView : View
{

    public TMP_InputField inputRoom;
    [SerializeField]
    private Button createRoomBtn;
    [SerializeField]
    private Button joinRoomBtn;
    [SerializeField]

    public Button RankBattleModeBtn;
    public Button NormalBattleModeBtn;

    //UI
    public RoomProfileUIBtn roomPrefabBtn;
    public Transform roomContent;

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

        NormalBattleModeBtn.onClick.AddListener(() =>
        {
            ViewManager.Instance.Show<NormalBattleView>();
        });

        RankBattleModeBtn.onClick.AddListener(() =>
        {
            ViewManager.Instance.Show<RankBattleView>();
        });

        base.Initialize();
    }

    public override void Show(object args = null)
    {
        base.Show(args);

        UpdateRoomProfileUI();
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

    public void UpdateRoomProfileUI()
    {
        foreach (Transform child in roomContent)
        {
            Destroy(child.gameObject);
        }

        foreach (RoomProfile roomProfile in Photon_Room_Manager.Instance.rooms)
        {
            //Chứa từ khóa ranking_ thì ẩn đi
            if (!roomProfile.name.Contains("ranking_"))
            {
                RoomProfileUIBtn roomProfileUIBtn = Instantiate(roomPrefabBtn);
                roomProfileUIBtn.SetRoomProfile(roomProfile);
                roomProfileUIBtn.transform.SetParent(roomContent);
            }

        }
    }

}
