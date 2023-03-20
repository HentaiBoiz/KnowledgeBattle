using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;

    const float StartTurnTime = 240f; //4p 1 turn

    //Biến đếm thời gian của Player 1 và 2
    public float player1Timer = 240f;
    public float player2Timer = 240f;

    public TextMeshProUGUI playerTimerTxt;
    public TextMeshProUGUI enemyTimerTxt;

    PhotonView _photonView;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _photonView = GetComponent<PhotonView>();

        player1Timer = player2Timer = StartTurnTime;
    }

    private void Update()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        if(TurnManager.Instance.currentTurn == 0) //Host Turn
        {
            player1Timer -= Time.deltaTime;

        }
        else
        {
            player2Timer -= Time.deltaTime;

        }

        UpdateTimerUI();
    }


    #region RPC METHOD
    public void ResetTimer(int index)
    {
        if (index == 0)
        {
            player1Timer = StartTurnTime;
        }
        else
        {
            player2Timer = StartTurnTime;
        }
        UpdateTimerUI();
    }

    //public void RPC_ResetTimer(int index)
    //{
    //    if (index == 0)
    //    {
    //        player1Timer = StartTurnTime;
    //    }
    //    else
    //    {
    //        player2Timer = StartTurnTime;
    //    }
    //    UpdateTimerUI();
    //}
    public void UpdateTimerUI()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            _photonView.RPC("RPC_UpdateTimerUI", RpcTarget.All, player1Timer, player2Timer);
        }
    }
    [PunRPC]
    public void RPC_UpdateTimerUI(float p1Time, float p2Time)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            playerTimerTxt.text = ((int)p1Time).ToString();
            enemyTimerTxt.text = ((int)p2Time).ToString();
        }
        else
        {
            playerTimerTxt.text = ((int)p2Time).ToString();
            enemyTimerTxt.text = ((int)p1Time).ToString();
        }
    }
    #endregion
}
