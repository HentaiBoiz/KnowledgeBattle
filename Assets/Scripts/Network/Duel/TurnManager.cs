using Photon.Pun;
using UnityEngine;
using TMPro;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance;
    PhotonView _photonView;

    public int totalTurn; //Tổng turn hiện tại
    public Transform DuelBackground;

    public enum TurnStep
    {
        DrawStep,
        ReadyStep,
        BattleStep,
        BonusStep,
        EndStep
    }

    public int currentTurn; //0: Host, 1: Client
    public int localSide; //Turn thật sự của người đang điều khiển game
    public TurnStep currentStep;


    [Header("TURN INTERFACT")]
    #region TURN INTERFACT
    public Transform drawButton;
    public Transform startBattleButton;
    public Transform bonusStepButton;
    public Transform endStepButton;
    #endregion

    [Header("CARD INTERFACT WITH TURN CHANGE")]
    public FieldQueueManager fieldQueueManager; //Trừ Lifetime của Support Card
    public FieldCheatManager fieldCheatManager; //Trừ Lifetime của Cheat Card

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _photonView = GetComponent<PhotonView>();

        //Set local turn
        if (PhotonNetwork.IsMasterClient)
        {
            localSide = 0;

            Vector3 to = new Vector3(0, 0, 0);
            DuelBackground.eulerAngles = Vector3.Lerp(DuelBackground.eulerAngles, to, 100f);
        }
        else
        {
            localSide = 1;

            Vector3 to = new Vector3(0, 180, 0);
            DuelBackground.eulerAngles = Vector3.Lerp(DuelBackground.eulerAngles, to, 100f);
        }

        SetTurnWhenStartGame();
    }

    private void Update()
    {
        if (Field_Manager_Id.Instance.isDuelStart == false)
            return;

        if (currentTurn != localSide)
            return;

        switch (currentStep)
        {
            case TurnStep.DrawStep:
                if (!drawButton.gameObject.activeSelf)
                    drawButton.gameObject.SetActive(true);
                
                break;
            case TurnStep.ReadyStep:
                if(totalTurn > 1)
                {
                    if (!startBattleButton.gameObject.activeSelf)
                        startBattleButton.gameObject.SetActive(true);
                }
                else
                {
                    if (!bonusStepButton.gameObject.activeSelf)
                        bonusStepButton.gameObject.SetActive(true);
                }
                break;
            case TurnStep.BattleStep:
                if (!bonusStepButton.gameObject.activeSelf)
                    bonusStepButton.gameObject.SetActive(true);
                break;
            case TurnStep.BonusStep:
                OnBonusStep();

                if (!endStepButton.gameObject.activeSelf)
                    endStepButton.gameObject.SetActive(true);
                break;
            case TurnStep.EndStep:
                ChangeTurn();
                break;
        }

    }

    #region RPC METHOD
    public void SetTurnWhenStartGame()
    {

        switch (PlayerInfoDDOL.Instance.playerProfile.goFirst)
        {
            case 1:
                if (PhotonNetwork.IsMasterClient)
                {
                    _photonView.RPC("RPC_SetTurnWhenStartGame", RpcTarget.All, 0);
                }
                else
                {
                    _photonView.RPC("RPC_SetTurnWhenStartGame", RpcTarget.All, 1);
                }
                break;
            case 2:
                if (PhotonNetwork.IsMasterClient)
                {
                    _photonView.RPC("RPC_SetTurnWhenStartGame", RpcTarget.All, 1);
                }
                else
                {
                    _photonView.RPC("RPC_SetTurnWhenStartGame", RpcTarget.All, 0);
                }
                break;

        }
    }
    [PunRPC]
    public void RPC_SetTurnWhenStartGame(int goFirst)
    {
        totalTurn = 1;
        currentTurn = goFirst;
        currentStep = TurnStep.DrawStep;
    }

    #endregion

    #region RPC STEP METHOD MANAGER
    public void GoToReadyStep() //Nếu muốn đi đến Ready Step thì phải rút bài trước
    {
        if (StateManager.Instance.DuelState != StateManager.ActionState.normalState)
            return;

        _photonView.RPC("RPC_GoToReadyStep", RpcTarget.All);

        if (currentTurn != localSide)
            return;

        Field_Manager_Id.Instance.DrawCard(localSide);
    }
    [PunRPC]
    public void RPC_GoToReadyStep()
    {
        currentStep = TurnStep.ReadyStep;
        drawButton.gameObject.SetActive(false);
    }

    public void GoToBattleStep() //Kết thúc Ready Step và tiến vào Battle Step
    {
        if (StateManager.Instance.DuelState != StateManager.ActionState.normalState)
            return;

        _photonView.RPC("RPC_GoToBattleStep", RpcTarget.All);

    }
    [PunRPC]
    public void RPC_GoToBattleStep()
    {
        currentStep = TurnStep.BattleStep;
        startBattleButton.gameObject.SetActive(false);
    }

    public void GoToBonusStep() //Kết thúc Battle Step và tiến vào Bonus Step
    {
        if (StateManager.Instance.DuelState != StateManager.ActionState.normalState)
            return;

        _photonView.RPC("RPC_GoToBonusStep", RpcTarget.All);

    }
    [PunRPC]
    public void RPC_GoToBonusStep()
    {
        currentStep = TurnStep.BonusStep;
        bonusStepButton.gameObject.SetActive(false);
    }

    public void GoToEndStep() //Kết thúc Bonus Step và chuyển sang End Step
    {
        if (StateManager.Instance.DuelState != StateManager.ActionState.normalState)
            return;

        _photonView.RPC("RPC_GoToEndStep", RpcTarget.All);

    }
    [PunRPC]
    public void RPC_GoToEndStep()
    {
        currentStep = TurnStep.EndStep;
        bonusStepButton.gameObject.SetActive(false);
    }

    public void ChangeTurn() //Kết thúc End Step và Switch Turn
    {
        if (StateManager.Instance.DuelState != StateManager.ActionState.normalState)
            return;

        //Set biến đã triệu hồi về false
        DuelRuleManager.Instance.wasPush = false;

        _photonView.RPC("RPC_ChangeTurn", RpcTarget.All);

    }
    [PunRPC]
    public void RPC_ChangeTurn()
    {
        ReduceLifeTime();
        
        totalTurn++; //Tăng tổng turn lên
        currentStep = TurnStep.DrawStep;

        if (currentTurn == 0)
            currentTurn = 1;
        else
            currentTurn = 0;

        //Reset thời gian
        TimeManager.Instance.ResetTimer(currentTurn);

        endStepButton.gameObject.SetActive(false);
    }
    #endregion

    #region EVENT METHOD
    public void OnBonusStep()
    {
        //Set attacked = false
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if(Field_Manager_Id.Instance.zoneId[i].battleZoneAttacked[j] == true)
                    Field_Manager_Id.Instance.SetAttackedBool(i, j, false);
            }
        }
    }
    #endregion

    #region ANOTHER METHOD


    //Reduce all Life time
    public void ReduceLifeTime()
    {
        foreach (var queue in fieldQueueManager.duelistQueues)
        {
            foreach (var card in queue.queueCards)
            {
                if (card.gameObject.activeSelf && card.cardMono != null && card.cardMono.id != "")
                {
                    if(card.cardMono.type == Card.CardType.SupportCard)
                        card.cardMono.timeLife--;
                }
            }
        }

        foreach (var cheat in fieldCheatManager.duelistCheats)
        {
            if (cheat.gameObject.activeSelf && cheat.GetComponent<ThisCard>().cardMono != null && cheat.GetComponent<ThisCard>().cardMono.id != "")
            {
                if (cheat.GetComponent<ThisCard>().cardMono.type == Card.CardType.CheatCard && !cheat.GetComponent<ThisCard>().isEnemyBack)
                    cheat.GetComponent<ThisCard>().cardMono.timeLife--;
            }
        }

        Field_Manager_Id.Instance.OnUpdateQueue();
        Field_Manager_Id.Instance.OnUpdateCheat();
    }
    #endregion
}
