using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using Photon.Pun;
using static UnityEngine.Rendering.DebugUI;
using UnityEngine.UI;


public class Rank : MonoBehaviour
{
    [SerializeField]
    public TMP_Text nickName;
    [SerializeField]
    public TMP_Text Ranking;
    [SerializeField]
    public TMP_Text KC;
    [SerializeField]
    public TMP_Text PlayerScore;

    [SerializeField]
    public Image RankImg;

    



    // Sprite Fresh1, Fresh2, Fresh3 , Soph1 , Soph2 , Soph3 , Junio1, Junio2 , Junio3 , Senio1 , Senio2, Senio3, Maste1 , Maste2, Maste3, Docto1 , Docto2 , Docto3, P1, P2, P3 = null;
   



    public void Start()
    {
        //GetPlayerCombined();

        nickName.text = PhotonNetwork.NickName;

        GetVirtualCurrency();
        //GetStatsRanking();
        //GetStatsScore();
        //GetLeaderBoard();
        GetRankingScore();

    }


    #region PLAYFAB METHOD


    private void RequestError(PlayFabError error)
    {
        string textERR = error.GenerateErrorReport();
        Debug.LogWarning(textERR);
    }


    #endregion
    public void GetPlayerCombined()
    {

        var request = new GetPlayerCombinedInfoRequestParams
        {
            GetPlayerProfile = true,

        };
    }
    public void GetName1(LoginResult re)
    {
        string name = null;
        name = re.InfoResultPayload.PlayerProfile.DisplayName;
        if (re.InfoResultPayload != null) { name = re.InfoResultPayload.PlayerProfile.DisplayName; }
        name = nickName.text;
    }

    public void GetStatsRanking()
    {
        var request = new GetPlayerStatisticsRequest();
        request.StatisticNames = new List<string>() { "Ranking" };
        PlayFabClientAPI.GetPlayerStatistics(request, GetStatRankingSuccess, OnPlayFabError);
    }
    public void GetStatsScore()
    {
        var request = new GetPlayerStatisticsRequest();
        request.StatisticNames = new List<string>() { "Score" };
        PlayFabClientAPI.GetPlayerStatistics(request, GetStatScoreSuccess, OnPlayFabError);
    }

    private void GetStatRankingSuccess(GetPlayerStatisticsResult obj)
    {
        print("success get");

        foreach (var st in obj.Statistics)
        {
           
            print(" Rank: " + st.Value);
          //  int ranks = st.Value;
           // Ranking.text = ranks.ToString();
        }
    }
    private void GetStatScoreSuccess(GetPlayerStatisticsResult statisticsResult)
    {
        print("success get score");

        foreach (var scr in statisticsResult.Statistics)
        {
           // PlayerScore.text = scr.Value.ToString();
            print(" Score: " + scr.Value);
            
        }
    }
    public void OnPlayFabError(PlayFabError e)
    {
        string er = e.GenerateErrorReport();
        Debug.LogWarning(er);
    }


     public void UpdatePlayerStatistics()
    {
        PlayFabClientAPI.UpdatePlayerStatistics(
            new UpdatePlayerStatisticsRequest()
            {
                Statistics = new List<StatisticUpdate>() {
                new StatisticUpdate() {
                    StatisticName = "Score",
                   // Version = 2,
                    Value = 50,

                }

                }
            },
            result => Debug.Log("Complete"),
            error => Debug.Log(error.GenerateErrorReport())
        );
    }

    void GetLeaderBoard()
    {

        var re = new GetLeaderboardRequest();
        re.StatisticName = "Score";
        re.Version = 1;
        re.StartPosition = 0;

        PlayFabClientAPI.GetLeaderboard(re, OnLeaderBoardSuccess, OnPlayFabError);
    }

    void OnLeaderBoardSuccess(GetLeaderboardResult obj)
    {

        foreach (var value in obj.Leaderboard)

        {
            int i = value.StatValue;
            //PlayerScore.text = (i%100).ToString() + " / 100";

        }
    }

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

    public void GetVirtualCurrency()
    {
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), OnGetUserCoinSuccess, OnRequestsError);
    }

    public void OnGetUserCoinSuccess(GetUserInventoryResult result)
    {
        
        int kc = result.VirtualCurrency["KC"];
        KC.text = kc.ToString();
    }

    public void OnRequestsError(PlayFabError error)
    {
        string textERR = error.GenerateErrorReport();
        Debug.LogWarning(textERR);
    }
    void UpdateRs(GetPlayerCombinedInfoResult result)
    {
        int value = 0;
        result.InfoResultPayload.UserVirtualCurrency.TryGetValue("FM", out value);
        Debug.Log(value);
       // Ranking.text = value.ToString();
    }

    public void ExitRank()
    {
        Debug.Log("Exit Rank");
        PhotonNetwork.LoadLevel("1_OfflineScene");
    }




}
