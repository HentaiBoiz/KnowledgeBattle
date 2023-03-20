using Photon.Pun;
using System;
using System.Collections;
using UnityEngine;
using static Card;

public class EffectManager : MonoBehaviour
{
    public static EffectManager Instance;
    PhotonView _photonView;

    //Identify Variable
    public int actionSide = -1; //Player nào vừa mới thực hiện hành động thì gán vào biến chung này
    public FieldQueueManager fieldQueueManager;
    public FieldBattleManager fieldBattleManager;
    public FieldCheatManager fieldCheatManager;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _photonView = GetComponent<PhotonView>();
    }


    #region EFFECT METHOD
    //Draw Card Effect
    public void StartDrawACard(int side)
    {
        StartCoroutine(DrawACard(side));
    }
    public IEnumerator DrawACard(int side)
    {
        yield return new WaitForSeconds(0.7f);
        Field_Manager_Id.Instance.DrawCard(side);
    }

    //Tăng LP theo lượng ATK của quái thú vừa mới triệu hồi
    public void IncreaseLPByLastedPushATK()
    {
        int currHp = Field_Manager_Id.Instance.zoneId[TurnManager.Instance.localSide].healthPoint;
        int gainAtk = CheatEventManager.Instance.GetLastPushCard().GetComponent<ThisCard>().cardMono.atk;

        Field_Manager_Id.Instance.SetDuelistHP(TurnManager.Instance.localSide, currHp + gainAtk);

        CheatEventManager.Instance.SetLastPushCard(-1, -1); //Có nghĩa là Null
    }
    #endregion

    #region RPC METHOD
    public void SetActionSide(int side)
    {
        _photonView.RPC("RPC_SetActionSide", RpcTarget.All, side);
    }
    [PunRPC]
    public void RPC_SetActionSide(int side)
    {
        actionSide = side;
    }

    //RPC EFFECT
    public void StartIncATKAttribute(int side, int amount, CardAttribute attribute, string affactId) //Tăng ATK của quái thú theo Attribute
    {
        _photonView.RPC("RPC_StartIncATKAttribute", RpcTarget.All, side, amount, attribute.ToString(), affactId);
    }
    [PunRPC]
    public void RPC_StartIncATKAttribute(int side, int amount, string attribute, string affactId)
    {
        CardAttribute _attribute = Enum.Parse<CardAttribute>(attribute);
        foreach (var battleCard in fieldBattleManager.duelistBattles[side].battleCards)
        {
            if (battleCard.gameObject.activeSelf)
            {
                if (battleCard.cardMono.attribute == _attribute)
                {
                    battleCard.GetComponent<CardInBattleZone>().affectingId.Add(affactId);
                    battleCard.cardMono.atk += amount;
                }
            }
        }

        Field_Manager_Id.Instance.Invoke("OnUpdateBattle", 0.2f);
    }
    //Giảm ATK
    public void EndIncATKAttribute(int side, int amount, CardAttribute attribute, string affactId) //Giảm ATK của quái thú theo Attribute
    {
        _photonView.RPC("RPC_EndIncATKAttribute", RpcTarget.All, side, amount, attribute.ToString(), affactId);
    }
    [PunRPC]
    public void RPC_EndIncATKAttribute(int side, int amount, string attribute, string affactId)
    {
        CardAttribute _attribute = Enum.Parse<CardAttribute>(attribute);
        foreach (var battleCard in fieldBattleManager.duelistBattles[side].battleCards)
        {
            if (battleCard.gameObject.activeSelf)
            {
                if (battleCard.cardMono.attribute == _attribute && battleCard.GetComponent<CardInBattleZone>().affectingId.Contains(affactId))
                {
                    battleCard.GetComponent<CardInBattleZone>().affectingId.Remove(affactId);
                    battleCard.cardMono.atk -= amount;
                }
            }
        }

        Field_Manager_Id.Instance.Invoke("OnUpdateBattle", 0.2f);
    }
    #endregion
}
