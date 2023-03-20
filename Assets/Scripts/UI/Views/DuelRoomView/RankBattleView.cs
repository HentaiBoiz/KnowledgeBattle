using Photon.Pun;
using Photon.Realtime;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RankBattleView : View
{
    [SerializeField]
    private Button rankDuelBtn;
    public TMP_Text Ranking;
    public Image RankImg;

    const int rankCondition = 200;

    public Button NormalModeBtn;
    public Button CustomModeBtn;

    public override void Initialize()
    {
        //Player rank duel
        rankDuelBtn.onClick.AddListener(() =>
        {
            StartCoroutine(RankMatchMaking());
        });

        NormalModeBtn.onClick.AddListener(() =>
        {
            ViewManager.Instance.Show<NormalBattleView>();
        });

        CustomModeBtn.onClick.AddListener(() =>
        {
            ViewManager.Instance.Show<CustomBattleView>();
        });

        GetRankingScore();

        base.Initialize();
    }

    public override void Show(object args = null)
    {
        base.Show(args);
    }

    

    #region METHOD BUTTONS
    IEnumerator RankMatchMaking()
    {
        rankDuelBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Finding...";
        yield return new WaitForSeconds(2f);

        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(),
            result =>
            {
                int rscore = result.VirtualCurrency["RS"]; //Lấy điểm Rank
                Debug.Log(rscore);

                Dictionary<string, int> rankRoomDic = new Dictionary<string, int>(); //Chứa các ranking room

                //Lấy các room và rank điều kiện của Room đó
                foreach (var room in Photon_Room_Manager.Instance.updatedRooms)
                {
                    Debug.Log(room.Name);
                    if (room.Name.Contains("ranking_"))
                    {
                        string rank = room.Name.Substring(8, 5);

                        rankRoomDic.Add(room.Name, StringToRankScore(rank));
                    }


                }
                List<string> matchConditions = new List<string>();
                //Loại bỏ các room có chênh lệch rank từ 200 trở lên
                foreach (var rankingRoom in rankRoomDic)
                {
                    if (Math.Abs(rscore - rankingRoom.Value) < rankCondition)
                    {
                        matchConditions.Add(rankingRoom.Key);
                    }
                }

                if (matchConditions.Count > 0) //Nếu có phòng
                {
                    Debug.Log("Match Found");
                    int rnd = UnityEngine.Random.Range(0, matchConditions.Count);

                    PhotonNetwork.JoinRoom(matchConditions[rnd]);
                }
                else
                {
                    Debug.Log("Match Not Found! Create New One");
                    CreateRankingRoom(rscore);
                }



            }, error =>
            {
                ErrorsManager.Instance.PushError(error.GenerateErrorReport());
            });

    }

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


    public void CreateRankingRoom(int Rscore)
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;

        float temp = Rscore / 1000f;

        PhotonNetwork.CreateRoom("ranking_" + temp.ToString("0.000") + "_" + DateTime.Now.ToOADate().ToString(), roomOptions, null);
    }



    //public void UpdateRoomProfileUI()
    //{
    //    foreach (Transform child in roomContent)
    //    {
    //        Destroy(child.gameObject);
    //    }

    //    foreach (RoomProfile roomProfile in rooms)
    //    {
    //        RoomProfileUIBtn roomProfileUIBtn = Instantiate(roomPrefabBtn);
    //        roomProfileUIBtn.SetRoomProfile(roomProfile);
    //        roomProfileUIBtn.transform.SetParent(roomContent);

    //    }
    //}
    #endregion


    //Rank Info
    public void GetRankingScore()
    {
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), OnGetUserInventorySuccess, OnRequestError);
    }

    public void OnGetUserInventorySuccess(GetUserInventoryResult result)
    {

        int rscore = result.VirtualCurrency["RS"];


        //Get Rank
        RankSpriteLoad rankSpriteLoad = new RankSpriteLoad();
        rankSpriteLoad.SetRank(rscore, Ranking, RankImg);

    }


    public void OnRequestError(PlayFabError error)
    {
        string textERR = error.GenerateErrorReport();
        Debug.LogWarning(textERR);

    }

}
