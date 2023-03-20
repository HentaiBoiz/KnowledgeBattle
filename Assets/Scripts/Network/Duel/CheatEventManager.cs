using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatEventManager : MonoBehaviour
{
    public static CheatEventManager Instance;
    PhotonView _photonView;

    #region MESSAGE 
    public Transform waitingCounterPopup;
    public Transform confirmCounterPopup;
    #endregion

    #region CHECKING VARIABLES
    private CardInBattleZone lastPushCard = null; //Card cuối cùng được người chơi Summon/ Push lên
    #endregion

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _photonView = GetComponent<PhotonView>();

        confirmCounterPopup.gameObject.SetActive(false);
        waitingCounterPopup.gameObject.SetActive(false);
    }

    private void Update()
    {
        //Show Cheat Confirm Popup again
        if (Input.GetMouseButtonDown(1) && StateManager.Instance.DuelState == StateManager.ActionState.counterState)
        {
            StartCounterConfirmState(TurnManager.Instance.currentTurn);
        }
    }

    #region GET SET METHOD
    public void SetLastPushCard(int side, int index)
    {
        if (side == -1)
            lastPushCard = null;
        else
            lastPushCard = EffectManager.Instance.fieldBattleManager.duelistBattles[side].battleCards[index].GetComponent<CardInBattleZone>();
    }
    public CardInBattleZone GetLastPushCard()
    {
        return lastPushCard;
    }
    #endregion


    #region LOCAL METHOD
    public void StartCounterConfirmState(int side)
    {
        int oppSide = 1 - side;

        if (Field_Manager_Id.Instance.zoneId[oppSide].cheatZone == null || Field_Manager_Id.Instance.zoneId[oppSide].cheatZone == "")
            return;



        StateManager.Instance.DuelState = StateManager.ActionState.confirmCounterState;

        if (TurnManager.Instance.localSide != side)
        {

            confirmCounterPopup.gameObject.SetActive(true);
        }
        else
        {
            waitingCounterPopup.gameObject.SetActive(true);
        }
    }

    public void StartCounterState()
    {
        StateManager.Instance.DuelState = StateManager.ActionState.counterState;
        confirmCounterPopup.gameObject.SetActive(false);
    }

    #endregion

    #region RPC METHOD
    public void EndCounterState()
    {
        _photonView.RPC("RPC_EndCounterState", RpcTarget.All);
    }
    [PunRPC]
    public void RPC_EndCounterState()
    {
        StateManager.Instance.DuelState = StateManager.ActionState.normalState;

        confirmCounterPopup.gameObject.SetActive(false);
        waitingCounterPopup.gameObject.SetActive(false);
    }

    //Set trạng thái cho lát bài vừa kích hoạt thành đã kích hoạt
    public void ActivatedCheat(int side)
    {
        _photonView.RPC("RPC_ActivatedCheat", RpcTarget.All, side);
    }
    [PunRPC]
    public void RPC_ActivatedCheat(int side)
    {
        EffectManager.Instance.fieldCheatManager.duelistCheats[side].isActivateEffect = true;
        Duel_VFX_Manager.Instance.CardAppearVFX(Field_Manager_Id.Instance.zoneId[side].cheatZone);
        EffectManager.Instance.fieldCheatManager.duelistCheats[side].GetComponent<ThisCard>().UnsetEnemyBack();
        
    }
    #endregion
}
