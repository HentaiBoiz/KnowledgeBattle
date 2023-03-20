using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.SocialPlatforms.Impl;
using System;

public class Handler : MonoBehaviour
{
    [SerializeField]
    public Slider slider;
    [SerializeField]
    public TMP_Text ScoreRank;
    // Start is called before the first frame update
    void Start()
    {
        slider.onValueChanged.AddListener((v) =>
        {
        slider.onValueChanged.AddListener((v) =>
        {
            GetRankingScore();
        //slider.onValueChanged.AddListener((v) =>
        //{
            
            
            
        });
        GetRankingScore();
        });
        //});
        GetRankingScore();
    }


    public void SetMinBar(int score)
    {
        slider.minValue = 0;
       
        slider.value = score;
        
        
    }


    public void GetRankingScore()
    {
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), OnGetUserCoinSuccess, OnRequestsError);
    }

    public void OnGetUserCoinSuccess(GetUserInventoryResult result)
    {

        int rs = result.VirtualCurrency["RS"];
        ScoreRank.text = Math.Abs(rs%100).ToString() + " / 100";
        slider.value = Math.Abs(rs % 100);
        slider.maxValue = 100;
        
        
    }

    public void OnRequestsError(PlayFabError error)
    {
        string textERR = error.GenerateErrorReport();
        Debug.LogWarning(textERR);
    }
   
}
