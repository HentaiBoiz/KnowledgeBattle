using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using UnityEngine.UI;
using PlayFab.ClientModels;
using TMPro;

public class script : MonoBehaviour
{


    [SerializeField]
    public Image RankImg;
    [SerializeField]
    public TMP_Text Ranking;


    string Freshman1 = "RankIm/FM1_1", Freshman2 = "RankIm/FM2", Freshman3 = "RankIm/FM3", Sopho1 = "RankIm/SP1",
       Sopho2 = "RankIm/SP2", Sopho3 = "RankIm/SP13", Junior1 = "RankIm/JU1_1", Junior2 = "RankIm/JU1_2", Junior3 = "RankIm/JU1_3", Senior1 = "RankIm/SE1", Senior2 = "RankIm/SE2", Senior3 = "RankIm/SE3",
       Master1 = "RankIm/MA1_1", Master2 = "RankIm/MA1_2", Master3 = "RankIm/MA1_3", Doctor1 = "RankIm/DO1", Doctor2 = "RankIm/DO2", Doctor3 = "RankIm/DO3", PD1 = "RankIm/PD1_1", PD2 = "RankIm/PD1_2", PD3 = "RankIm/PD1_3";

    public void GetRankingScore()
    {
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), OnGetUserInventorySuccess, OnRequestError);
    }

    public void OnGetUserInventorySuccess(GetUserInventoryResult result)
    {

        int rscore = result.VirtualCurrency["RS"];


        //Get Rank
        if (rscore >= 0 && rscore < 100)
        {
            Debug.Log("Rank: Freshman");
            Ranking.text = "Freshman I";
            RankImg.sprite = Resources.Load<Sprite>(Freshman1);

        }
        else if (rscore >= 100 && rscore < 200)
        {
            Debug.Log("Rank: Freshman");
            Ranking.text = "Freshman II";
            RankImg.sprite = Resources.Load<Sprite>(Freshman2);

        }
        else if (rscore >= 200 && rscore < 300)
        {
            Debug.Log("Rank: Freshman");
            Ranking.text = "Freshman III";
            RankImg.sprite = Resources.Load<Sprite>(Freshman3);

        }
        else if (rscore >= 300 && rscore < 400)
        {
            Debug.Log("Rank: Sophomore");
            Ranking.text = "Sophomore I";
            RankImg.sprite = Resources.Load<Sprite>(Sopho1);

        }
        else if (rscore >= 400 && rscore < 500)
        {
            Debug.Log("Rank: Sophomore");
            Ranking.text = "Sophomore II";
            RankImg.sprite = Resources.Load<Sprite>(Sopho2);

        }
        else if (rscore >= 500 && rscore < 600)
        {
            Debug.Log("Rank: Sophomore");
            Ranking.text = "Sophomore III";
            RankImg.sprite = Resources.Load<Sprite>(Sopho3);

        }
        else if (rscore >= 600 && rscore < 700)
        {
            Debug.Log("Rank: Junior");
            Ranking.text = "Junior I";
            RankImg.sprite = Resources.Load<Sprite>(Junior1);

        }
        else if (rscore >= 700 && rscore < 800)
        {
            Debug.Log("Rank: Junior");
            Ranking.text = "Junior II";
            RankImg.sprite = Resources.Load<Sprite>(Junior2);


        }
        else if (rscore >= 800 && rscore < 900)
        {
            Debug.Log("Rank: Junior");
            Ranking.text = "Junior III";
            RankImg.sprite = Resources.Load<Sprite>(Junior3);


        }
        else if (rscore >= 1000 && rscore < 1100)
        {
            Debug.Log("Rank: Senior");
            Ranking.text = "Senior I";
            RankImg.sprite = Resources.Load<Sprite>(Senior1);


        }
        else if (rscore >= 1100 && rscore < 1200)
        {
            Debug.Log("Rank: Senior");
            Ranking.text = "Senior II";
            RankImg.sprite = Resources.Load<Sprite>(Senior2);


        }
        else if (rscore >= 1200 && rscore < 1300)
        {
            Debug.Log("Rank: Senior");
            Ranking.text = "Senior III";
            RankImg.sprite = Resources.Load<Sprite>(Senior3);


        }
        else if (rscore >= 1300 && rscore < 1400)
        {
            Debug.Log("Rank: Master");
            Ranking.text = "Master I";
            RankImg.sprite = Resources.Load<Sprite>(Master1);

        }
        else if (rscore >= 1400 && rscore < 1500)
        {
            Debug.Log("Rank: Master");
            Ranking.text = "Master II";
            RankImg.sprite = Resources.Load<Sprite>(Master2);

        }
        else if (rscore >= 1500 && rscore < 1600)
        {
            Debug.Log("Rank: Master");
            Ranking.text = "Master III";
            RankImg.sprite = Resources.Load<Sprite>(Master3);

        }
        else if (rscore >= 1600 && rscore < 1700)
        {
            Debug.Log("Rank: Doctor");
            Ranking.text = "Doctor I";
            RankImg.sprite = Resources.Load<Sprite>(Doctor1);

        }
        else if (rscore >= 1700 && rscore < 1800)
        {
            Debug.Log("Rank: Doctor");
            Ranking.text = "Doctor II";
            RankImg.sprite = Resources.Load<Sprite>(Doctor2);

        }
        else if (rscore >= 1800 && rscore < 1900)
        {
            Debug.Log("Rank: Doctor");
            Ranking.text = "Doctor III";
            RankImg.sprite = Resources.Load<Sprite>(Doctor3);

        }
        else if (rscore >= 1900 && rscore < 2000)
        {
            Debug.Log("Rank: Post-doctoral");
            Ranking.text = "Post-doctoral I";
            RankImg.sprite = Resources.Load<Sprite>(PD1);

        }
        else if (rscore >= 2000 && rscore < 2100)
        {
            Debug.Log("Rank: Post-doctoral");
            Ranking.text = "Post-doctoral II";
            RankImg.sprite = Resources.Load<Sprite>(PD2);

        }
        else if (rscore >= 2100 && rscore < 2200)
        {
            Debug.Log("Rank: Post-doctoral");
            Ranking.text = "Post-doctoral III";
            RankImg.sprite = Resources.Load<Sprite>(PD3);

        }
    }


    public void OnRequestError(PlayFabError error)
    {
        string textERR = error.GenerateErrorReport();
        Debug.LogWarning(textERR);

    }


    // Start is called before the first frame update
    void Start()
    {
        GetRankingScore();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
