using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnpackCard : DeckEditorCard
{
    protected override void WhenLeftMouseClick()
    {
        base.WhenLeftMouseClick();

        if (cardBack.gameObject.activeSelf && PlayfabCardDB.Instance.playfabCardDB[cardId].cardSprite != null)
        {
            cardImage.sprite = PlayfabCardDB.Instance.playfabCardDB[cardId].cardSprite;
            GetComponent<Animation>().Play();
        }
            
    }

}
