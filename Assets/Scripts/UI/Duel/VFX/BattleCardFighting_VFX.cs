using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCardFighting_VFX : MonoBehaviour
{
    public CardUI_VFX attackCard;
    public CardUI_VFX defendCard;

    public void SetUpFightingCard(string attackId, string defendId)
    {
        //Setup Card Detail
        attackCard.ShowCardDetail(attackId);
        defendCard.ShowCardDetail(defendId);
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }
}
