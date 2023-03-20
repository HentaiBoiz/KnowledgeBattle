using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Duel_VFX_Manager : MonoBehaviour
{
    public static Duel_VFX_Manager Instance;

    //VFX Transform
    public FieldBattleManager fieldBattleManager;

    //VFX Objects
    [Header("CARD APPEAR")]
    public CardUI_VFX cardAppearVFX;
    public CardUI_VFX battleCardAppearVFX;
    [Header("BATTLE VFX")]
    public BattleCardFighting_VFX playerAttackOpp_VFX;
    public BattleCardFighting_VFX oppAttackPlayer_VFX;
    public CardUI_VFX playerAttackDirectly_VFX;
    public CardUI_VFX oppAttackDirectly_VFX;

    PhotonView _photonView;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _photonView = GetComponent<PhotonView>();

        //Disable VFX
        DisableVFX();
    }

    #region RPC METHOD
    public void StartAttackVFX(string attackId, string defendId, int attackerSide)
    {
        _photonView.RPC("RPC_StartAttackVFX", RpcTarget.All, attackId, defendId, attackerSide);
    }
    [PunRPC]
    public void RPC_StartAttackVFX(string attackId, string defendId, int attackerSide)
    {
        if (attackerSide == TurnManager.Instance.localSide)
        {
            playerAttackOpp_VFX.SetUpFightingCard(attackId, defendId);
            playerAttackOpp_VFX.gameObject.SetActive(true);
        }
        else
        {
            oppAttackPlayer_VFX.SetUpFightingCard(attackId, defendId);
            oppAttackPlayer_VFX.gameObject.SetActive(true);
        }
        
    }
    //Attack Directly
    public void StartAttackDirectlyVFX(string attackId, int attackerSide)
    {
        _photonView.RPC("RPC_StartAttackDirectlyVFX", RpcTarget.All, attackId, attackerSide);
    }
    [PunRPC]
    public void RPC_StartAttackDirectlyVFX(string attackId, int attackerSide)
    {
        if (attackerSide == TurnManager.Instance.localSide)
        {
            playerAttackDirectly_VFX.ShowCardDetail(attackId);
            playerAttackDirectly_VFX.gameObject.SetActive(true);
        }
        else
        {
            oppAttackDirectly_VFX.ShowCardDetail(attackId);
            oppAttackDirectly_VFX.gameObject.SetActive(true);
        }

    }

    #endregion

    #region LOCAL METHOD
    //Queue Card VFX
    public void CardAppearVFX(string cardId)
    {
        cardAppearVFX.gameObject.SetActive(false);
        cardAppearVFX.ShowCardDetail(cardId);
        StartCoroutine(CardAppear());
    }
    IEnumerator CardAppear()
    {
        cardAppearVFX.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        cardAppearVFX.gameObject.SetActive(false);
    }
    //Push Battle Card VFX
    public void BattleCardAppearVFX(string cardId)
    {
        battleCardAppearVFX.gameObject.SetActive(false);
        battleCardAppearVFX.ShowCardDetail(cardId);
        StartCoroutine(BattleCardAppear());
    }
    IEnumerator BattleCardAppear()
    {
        battleCardAppearVFX.gameObject.SetActive(true);
        yield return new WaitForSeconds(5f);
        battleCardAppearVFX.gameObject.SetActive(false);
    }
    public void DisableVFX()
    {
        cardAppearVFX.gameObject.SetActive(false);
        playerAttackOpp_VFX.gameObject.SetActive(false);
        oppAttackPlayer_VFX.gameObject.SetActive(false);
        playerAttackDirectly_VFX.gameObject.SetActive(false);
        oppAttackDirectly_VFX.gameObject.SetActive(false);
    }
    #endregion
}
