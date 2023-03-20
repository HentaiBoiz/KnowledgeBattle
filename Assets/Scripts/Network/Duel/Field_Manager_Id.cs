using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;
using UnityEngine;

public class Field_Manager_Id : MonoBehaviour
{
    public static Field_Manager_Id Instance;
    const int StartGameHP = 200; //Lượng máu đầu game nhận được

    public ZoneId[] zoneId = new ZoneId[2]; //2 Object chứa id của các lá bài

    PhotonView _photonView;

    public bool isDuelStart = false;

    #region UPDATE TRIGGER FOR LOCAL
    float updateTime = 15f; //Sau 15s thì Update 1 lần
    float countTime = 0f;
    [HideInInspector]
    public bool isUpdateDeck = false;
    [HideInInspector]
    public bool isUpdateHand = false;
    [HideInInspector]
    public bool isUpdateQueue = false;
    [HideInInspector]
    public bool isUpdateBattle = false;
    [HideInInspector]
    public bool isUpdateDrop = false;
    [HideInInspector]
    public bool isUpdateCheat = false;
    #endregion

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _photonView = GetComponent<PhotonView>();

        PhotonDuelistStats.Instance.LoadRoomDuelists(); //Load Stats của Player
    }

    private void Update()
    {
        countTime -= Time.deltaTime;
        if (countTime > 0f)
            return;

        countTime = updateTime;

        OnUpdateBattle();
        OnUpdateCheat();
        OnUpdateQueue();
    }

    #region CHECKING METHOD
    public int CheckBattleZoneEmpty(int side)
    {
        for (int i = 0; i < 3; i++)
        {
            if (zoneId[side].battleZone[i] == "")
            {
                return i;
            }
                
        }

        return -1; //Không có Battle Zone nào còn trống
    }

    //Check thử side đó có Battle Card hay không, ko có thì direct attack
    public bool CheckOppBattleIsEmpty(int side)
    {
        for (int i = 0; i < 3; i++)
        {
            if (!(zoneId[side].battleZone[i] == ""))
            {
                return false;
            }

        }

        return true; //Không có Battle Zone nào còn trống
    }
    #endregion


    #region RPC METHOD
    public void UpdateDeck(string[] deckIds,int side)
    {
        _photonView.RPC("RPC_UpdateDeck", RpcTarget.All, deckIds, side);
    }
    [PunRPC]
    public void RPC_UpdateDeck(string[] deckIds,int side)
    {
        zoneId[side].deckZone = deckIds.ToList();
        
    }
    public void DrawCard(int side)
    {
        _photonView.RPC("RPC_DrawCard", RpcTarget.All, side);
    }
    [PunRPC]
    public void RPC_DrawCard(int side)
    {
        int currCard = zoneId[side].deckZone.Count - 1;

        string transCardId = zoneId[side].deckZone[currCard];
        zoneId[side].handZone.Add(transCardId);

        zoneId[side].deckZone.RemoveAt(currCard);

        Invoke("OnUpdateHand", 0.2f);
        Invoke("OnUpdateDeck", 0.2f);
    }

    public void QueueACard()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < zoneId[0].queueZone.Length; i++)
            {
                if (zoneId[0].queueZone[i] == "")
                {

                    EffectManager.Instance.SetActionSide(0); //Host là người thực hiện hành động Queue này

                    Question_Manager_Id.Instance.SetQuestion(0, i, SelectionManager.Instance.handPopup.handIndex); //Set câu hỏi

                    _photonView.RPC("RPC_QueueACard", RpcTarget.All, 0, i, SelectionManager.Instance.handPopup.handIndex);

                    break;
                }
            }
        }
        else
        {
            for (int i = 0; i < zoneId[1].queueZone.Length; i++)
            {
                if (zoneId[1].queueZone[i] == "")
                {
                    EffectManager.Instance.SetActionSide(1); //Client là người thực hiện hành động Queue này

                    Question_Manager_Id.Instance.SetQuestion(1, i, SelectionManager.Instance.handPopup.handIndex); //Set câu hỏi

                    _photonView.RPC("RPC_QueueACard", RpcTarget.All, 1, i, SelectionManager.Instance.handPopup.handIndex);

                    break;
                }
            }
        }

    }
    [PunRPC]
    public void RPC_QueueACard(int side, int zoneIndex, int handIndex)
    {
        zoneId[side].queueZone[zoneIndex] = zoneId[side].handZone[handIndex];

        Duel_VFX_Manager.Instance.CardAppearVFX(zoneId[side].queueZone[zoneIndex]);

        zoneId[side].handZone.RemoveAt(handIndex);


        Invoke("OnUpdateHand", 0.2f);
        Invoke("OnUpdateQueue", 1f);

    }

    public void ActivateSupportCard()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < zoneId[0].queueZone.Length; i++)
            {
                if (zoneId[0].queueZone[i] == "")
                {
                    EffectManager.Instance.SetActionSide(0); //Host là người thực hiện hành động Queue này

                    _photonView.RPC("RPC_ActivateSupportCard", RpcTarget.All, 0, i, SelectionManager.Instance.handPopup.handIndex);
                    break;
                }
            }
        }
        else
        {
            for (int i = 0; i < zoneId[1].queueZone.Length; i++)
            {
                if (zoneId[1].queueZone[i] == "")
                {
                    EffectManager.Instance.SetActionSide(1); //Client là người thực hiện hành động Queue này

                    _photonView.RPC("RPC_ActivateSupportCard", RpcTarget.All, 1, i, SelectionManager.Instance.handPopup.handIndex);
                    break;
                }
            }
        }
    }
    [PunRPC]
    public void RPC_ActivateSupportCard(int side, int zoneIndex, int handIndex)
    {
        zoneId[side].queueZone[zoneIndex] = zoneId[side].handZone[handIndex];

        Duel_VFX_Manager.Instance.CardAppearVFX(zoneId[side].queueZone[zoneIndex]);

        zoneId[side].handZone.RemoveAt(handIndex);

        Invoke("OnUpdateHand", 0.2f);
        Invoke("OnUpdateQueue", 1f);
    }

    public void ActivateCheatCard()
    {
        EffectManager.Instance.fieldCheatManager.duelistCheats[TurnManager.Instance.localSide].CheckCheatEffect(EffectManager.Instance);

        Invoke("OnUpdateHand", 0.2f);
        Invoke("OnUpdateCheat", 0.2f);
        
    }
    [PunRPC]
    public void RPC_ActivateCheatCard(int side)
    {
        EffectManager.Instance.fieldCheatManager.duelistCheats[side].CheckCheatEffect(EffectManager.Instance);

        Invoke("OnUpdateHand", 0.2f);
        Invoke("OnUpdateCheat", 0.2f);
    }

    public void SetCheatCard()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            EffectManager.Instance.SetActionSide(0); //Host là người thực hiện hành động Cheat này

            _photonView.RPC("RPC_SetCheatCard", RpcTarget.All, 0, SelectionManager.Instance.handPopup.handIndex);
        }
        else
        {
            EffectManager.Instance.SetActionSide(1); //Client là người thực hiện hành động Cheat này

            _photonView.RPC("RPC_SetCheatCard", RpcTarget.All, 1, SelectionManager.Instance.handPopup.handIndex);
            
        }
    }
    [PunRPC]
    public void RPC_SetCheatCard(int side, int handIndex)
    {
        if (zoneId[side].cheatZone == "")
        {
            zoneId[side].cheatZone = zoneId[side].handZone[handIndex];
            zoneId[side].handZone.RemoveAt(handIndex);
        }
        else
        {
            zoneId[side].handZone.Add(zoneId[side].cheatZone);

            zoneId[side].cheatZone = zoneId[side].handZone[handIndex];
            zoneId[side].handZone.RemoveAt(handIndex);
        }
        

        Invoke("OnUpdateHand", 0.2f);
        Invoke("OnUpdateCheat", 0.2f);
    }

    //HP
    public void SetDuelistHP(int side, int hpSet)
    {
        _photonView.RPC("RPC_SetDuelistHP",RpcTarget.All, side, hpSet);
    }
    [PunRPC]
    public void RPC_SetDuelistHP(int side, int hpSet)
    {
        zoneId[side].healthPoint = hpSet;
    }

    //Set Attacked Bool
    public void SetAttackedBool(int side, int index,bool attacked)
    {
        _photonView.RPC("RPC_SetAttackedBool", RpcTarget.All, side, index, attacked);
    }
    [PunRPC]
    public void RPC_SetAttackedBool(int side, int index,bool attacked)
    {
        zoneId[side].battleZoneAttacked[index] = attacked;
    }
    #endregion


    #region LOCAL METHOD
    //Chuẩn bị Deck để đấu
    public void SetDeckReady() 
    {

        if (PhotonNetwork.IsMasterClient)
        {
            UpdateDeck(PhotonDuelistStats.Instance.deckJson.cardIds.ToArray(), 0);
            OnUpdateDeck();
        }
        else
        {
            UpdateDeck(PhotonDuelistStats.Instance.deckJson.cardIds.ToArray(), 1);
            OnUpdateDeck();
        }

        OnUpdateDeck();
    }

    public void StartDuel()
    {
        //Mới đầu thì Set Hp = -1 để khỏi phải chạy điều kiện bên HealthPointManager
        isDuelStart = true;

        //Set hình phạt cho Player nếu thoát trận
        PlayfabUserInfomation.Instance.SetPlayerPunish(Random.Range(40, 50));

        //Draw 5 Card
        if (PhotonNetwork.IsMasterClient)
        {
            SetDuelistHP(0, StartGameHP);

            DrawXCards(0, 5);   
        }
        else
        {
            SetDuelistHP(1, StartGameHP);

            DrawXCards(1, 5);
        }
    }

    public void DrawXCards(int side, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            DrawCard(side);
        }  
    }

    //Kiểm tra zone đã full hay chưa ?
    public bool queueZoneIsFull(int side)
    {
        for (int i = 0; i < zoneId[side].queueZone.Length; i++)
        {
            if (zoneId[side].queueZone[i] == "")
            {
                return false;
            }
        }

        return true;
    }

    //Drop bài vào Drop Zone
    public void AddCardToDropZone(int side, string cardId)
    {
        zoneId[side].dropZone.Add(cardId);
        Invoke("OnUpdateDrop", 0.2f);
    }

    #endregion


    #region RAISED EVENT METHOD
    public void OnUpdateDeck()
    {
        object[] datas = new object[] { }; //Đóng gói

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };

        PhotonNetwork.RaiseEvent(
            ((byte)EventCode.onUpdateDeck),
            datas,
            raiseEventOptions,
            SendOptions.SendReliable);

    }
    public void OnUpdateHand()
    {
        object[] datas = new object[] { }; //Đóng gói

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };

        PhotonNetwork.RaiseEvent(
            ((byte)EventCode.onUpdateHand),
            datas,
            raiseEventOptions,
            SendOptions.SendReliable);

    }

    public void OnUpdateQueue()
    {
        object[] datas = new object[] { }; //Đóng gói

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };

        PhotonNetwork.RaiseEvent(
            ((byte)EventCode.onUpdateQueue),
            datas,
            raiseEventOptions,
            SendOptions.SendReliable);

    }

    public void OnUpdateBattle()
    {
        object[] datas = new object[] { }; //Đóng gói

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };

        PhotonNetwork.RaiseEvent(
            ((byte)EventCode.onUpdateBattle),
            datas,
            raiseEventOptions,
            SendOptions.SendReliable);

    }

    public void OnUpdateDrop()
    {
        object[] datas = new object[] { }; //Đóng gói

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };

        PhotonNetwork.RaiseEvent(
            ((byte)EventCode.onUpdateDrop),
            datas,
            raiseEventOptions,
            SendOptions.SendReliable);

    }

    public void OnUpdateCheat()
    {
        object[] datas = new object[] { }; //Đóng gói

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };

        PhotonNetwork.RaiseEvent(
            ((byte)EventCode.onUpdateCheat),
            datas,
            raiseEventOptions,
            SendOptions.SendReliable);

    }
    #endregion


}
